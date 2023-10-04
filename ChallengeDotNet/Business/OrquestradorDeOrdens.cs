using System;
using System.Collections.Concurrent;
using ChallengeDotNet.Repositories;
using ChallengeDotNet.Domain;

namespace ChallengeDotNet.Business
{
    public class OrquestradorDeOrdens
    {

        private static List<Ordem> OrdensEmMemoria = new List<Ordem>();
        private static List<User> Usuarios = new List<User>();

        private static ConcurrentQueue<Ordem> RequisicaoDeOrdem = new ConcurrentQueue<Ordem>();

        public static Repository _repository;

        public OrquestradorDeOrdens()
        {
            _repository = new Repository();

            PopulateOrdersAndUsers();
            PopulateRequests();

            Console.WriteLine("Done");
        }

        public void ProcessIncomingRequests()
        {
            while (RequisicaoDeOrdem.TryDequeue(out Ordem requisicaoDeOrdem))
                ValidateOrder(requisicaoDeOrdem);
        }

        public static bool ValidateOrder(Ordem orderRequest)
        {
            Ordem o;
            User u;

            lock (OrdensEmMemoria)
            {
                lock (Usuarios)
                {
                    o = OrdensEmMemoria.FindAll(o => o.OrderID == orderRequest.OrderID).FirstOrDefault();

                    u = Usuarios.FindAll(u => u.UserID == o?.UserID).FirstOrDefault();

                    if (u?.Active == true && o != null)
                    {
                        Clone(orderRequest, o);
                        AtualizarOrdem(orderRequest);

                    } else if (u?.Active == true && o == null) {
                        OrdensEmMemoria.Add(orderRequest);
						PersistirOrdem(orderRequest);
                    }
				}
            }

            return u.Active;
        }

        private static void Clone(Ordem orderRequest, Ordem inMemory)
        {
            // Clona propriedades do orderRequest para o inMemory
        }

        private static void AtualizarOrdem(Ordem ordem)
        {
            _repository.Update(ordem);
        }

		private static void PersistirOrdem(Ordem ordem)
		{
			_repository.Save(ordem);
		}

		public static void PopulateRequests()
        {
            foreach (var order in OrdensEmMemoria)
                RequisicaoDeOrdem.Enqueue(new Ordem() { OrderID = order.OrderID });
        }

        public static void PopulateOrdersAndUsers()
        {
            for (var i = 1; i <= 100000; i++)
            {
                var order = new Ordem() { OrderID = i, Symbol = "PETR" + i % 10, UserID = i };
                var user = new User() { UserID = i, Name = "User" + i };

                OrdensEmMemoria.Add(order);
                Usuarios.Add(user);
            }
        }
    }
}
