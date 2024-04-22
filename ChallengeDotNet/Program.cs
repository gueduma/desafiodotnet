using ChallengeDotNet.Business;

var orquestrador = new OrquestradorDeOrdens();

var task1 = Task.Run(() => orquestrador.AddReqs());

var task2 = Task.Run(() => orquestrador.ProcessIncomingRequests());

var task3 = Task.Run(() => orquestrador.ProcessIncomingRequests());

await Task.WaitAll(task1, task2, task3);

Console.WriteLine("Fim");
