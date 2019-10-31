using UnityEngine;
using System.Collections;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DBAccess : MonoBehaviour
{
    private string connection;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;

    public void OnApplicationStart()
    {
        // check if file exists in Application.persistentDataPath
        string filepath = Application.persistentDataPath + "/" + "neverendingLocal.db";
        
        string path = "";

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.streamingAssetsPath + "/neverendingLocal.db";
            filepath = path;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            path = "jar:file://" + Application.dataPath + "!/assets/" + "neverendingLocal.db";
            WWW loadDB = new WWW(path);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            path = "file://" + Application.dataPath + "/Raw/" + "neverendingLocal.db";
            WWW loadDB = new WWW(path);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }

        
    }

    private void OpenDatabase() {
        // check if file exists in Application.persistentDataPath

        string filepath = "";

        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            filepath = Application.persistentDataPath + "/" + "neverendingLocal.db";
        }
        else
        {
            filepath = Application.streamingAssetsPath + "/neverendingLocal.db";
        }

        if (!File.Exists(filepath))
        {
            // if it doesn't, open StreamingAssets directory and load the db
            // This is simply a backup. This will almost always (if not always) be bypassed.
            string path = "";

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = Application.streamingAssetsPath + "/neverendingLocal.db";
                filepath = path;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                path = "jar:file://" + Application.dataPath + "!/assets/" + "neverendingLocal.db";
                WWW loadDB = new WWW(path);
                while (!loadDB.isDone) { }
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                path = "file://" + Application.dataPath + "/Raw/" + "neverendingLocal.db";
                WWW loadDB = new WWW(path);
                while (!loadDB.isDone) { }
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
        }
        //open db connection
        connection = "URI=file:" + filepath;
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        dbcmd = dbcon.CreateCommand();
    }

    private void CloseDatabase()
    {
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }

    public Enemy GetEnemyInfo(string _name) {
        string enemyName = "";
        int health = 0;
        double speed = 0;
        double attack = 0;

        OpenDatabase();

        string sql = "SELECT * FROM enemies WHERE name IS '" + _name + "'";

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            enemyName = reader.GetString(1);
            health = reader.GetInt32(2);
            speed = reader.GetDouble(3);
            attack = reader.GetDouble(4);
        }

        // clean up
        CloseDatabase();

        Enemy enemy = new Enemy(enemyName, health, speed, attack);
        return enemy;

    }

    public int GetLootTableSize() {

        string tableName = "loot";
        int lootCount = 0;

        OpenDatabase();

        string sql = "SELECT COUNT(*) FROM '" + tableName + "'";

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            lootCount = reader.GetInt32(0);
        }

        // clean up
        CloseDatabase();

        return lootCount;
    }

    public Loot GetLootDrop(int rowId)
    {
        string tableName = "loot";
        string itemName = "";
        int itemValue = 0;

        OpenDatabase();

        string sql = "SELECT * FROM '" + tableName + "' WHERE id IS " + rowId;

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            itemName = reader.GetString(1);
            itemValue = reader.GetInt32(2);
        }

        CloseDatabase();

        return new Loot(itemName, itemValue);
    }

    // This takes the chosen class and pulls the information from the database
    public ClassObject GetClassInfo(string _chosenClassName)
    {
        string tableName = "PlayerClass";
        string className = "";
        int classHealth = 0;
        int classSpeed = 0;
        int projectileSpeed = 0;
        int projectileDamage = 0;

        OpenDatabase();

        string sql = "SELECT * FROM '" + tableName + "' WHERE name IS '" + _chosenClassName + "'";

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            className = reader.GetString(0);
            classHealth = reader.GetInt32(1);
            classSpeed = reader.GetInt32(2);
            projectileSpeed = reader.GetInt32(3);
            projectileDamage = reader.GetInt32(4);
        }

        CloseDatabase();
        
        return new ClassObject(className, classHealth, classSpeed, projectileSpeed, projectileDamage);
    }

    public int GetBossTableSize()
    {

        string tableName = "bosses";
        int lootCount = 0;

        OpenDatabase();

        string sql = "SELECT COUNT(*) FROM '" + tableName + "'";

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            lootCount = reader.GetInt32(0);
        }

        // clean up
        CloseDatabase();

        return lootCount;
    }

    public Boss GetChosenBoss(int rowId)
    {
        string tableName = "bosses";
        string bossName = "";
        int baseHealth = 0;
        int baseAttackDamageOne = 0;
        int baseAttackDamageTwo = 0;

        OpenDatabase();

        string sql = "SELECT * FROM '" + tableName + "' WHERE id IS " + rowId;

        dbcmd.CommandText = sql;

        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            bossName = reader.GetString(1);
            baseHealth = reader.GetInt32(2);
            baseAttackDamageOne = reader.GetInt32(3);
            baseAttackDamageTwo = reader.GetInt32(4);
        }

        CloseDatabase();

        return new Boss(bossName, baseHealth, baseAttackDamageOne, baseAttackDamageTwo);
    }

}