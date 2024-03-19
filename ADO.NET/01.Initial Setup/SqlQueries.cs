using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.Initial_Setup
{
    public class SqlQueries
    {
       public const string GetVillainsWithNumberOfMinions =
            @"SELECT [Name],COUNT(*) AS TotalMinions
            FROM Villains AS v
            JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
            GROUP BY [Name]
            HAVING COUNT(*) > 3
            ORDER BY TotalMinions";

    }
}
