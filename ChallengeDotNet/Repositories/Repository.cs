using ChallengeDotNet.Domain;

namespace ChallengeDotNet.Repositories;

public class Repository
{
    public static List<Ordem> ordensPersistidas = new List<Ordem>();

	public int Save(Ordem ordem)
    {
        ordensPersistidas.Add(ordem);

        return 1;
    }
}