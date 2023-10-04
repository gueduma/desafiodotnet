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

	public int Update(Ordem ordem)
	{
		ordensPersistidas.RemoveAll(x => x.OrderID == ordem.OrderID);
		ordensPersistidas.Add(ordem);

		return 1;
	}
}