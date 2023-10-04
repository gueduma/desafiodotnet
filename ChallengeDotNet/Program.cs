using ChallengeDotNet.Business;

var orquestrador = new OrquestradorDeOrdens();

orquestrador.ProcessIncomingRequests();

Console.WriteLine("Fim");