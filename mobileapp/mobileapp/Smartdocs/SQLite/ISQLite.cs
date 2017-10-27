using System;
using SQLite;

namespace Smartdocs
{
	public interface ISQLite
	{        
		SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
        String GetDatabasePath();
	}    
}
