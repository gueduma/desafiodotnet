using System;
using System.Collections.Concurrent;
using ChallengeDotNet.Repositories;
using ChallengeDotNet.Domain;
using System.Runtime.Intrinsics.Arm;

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

            Console.WriteLine("Done");
        }

        public void ProcessIncomingRequests()
        {
            while (RequisicaoDeOrdem.TryDequeue(out Ordem requisicaoDeOrdem))
                ValidateOrder(requisicaoDeOrdem);
        }

        public static bool? ValidateOrder(Ordem orderRequest)
        {
            Ordem o;
            User u;

            lock (OrdensEmMemoria)
            {
                lock (Usuarios)
                {
                    o = OrdensEmMemoria.FindAll(o => o.OrderID == orderRequest.OrderID).FirstOrDefault();

                    u = Usuarios.FindAll(u => u.UserID == orderRequest?.UserID).FirstOrDefault();

                    if (u?.Active == true && o != null)
                    {
                        Console.WriteLine("Ordem já existente!");
                        // Atualiza na memória e no banco

                    } else if (u?.Active == true && o == null) {
						Console.WriteLine("Ordem nova!");

						OrdensEmMemoria.Add(orderRequest);
						_repository.Save(o);
					}
				}
            }

            return u?.Active;
        }

    }
}
