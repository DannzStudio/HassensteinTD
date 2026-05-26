using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class LevelManager
{
    public LevelData CurrentLevel;

    Form1 form1;

    public LevelManager(Form1 form)
    {
        form1 = form;
    }

    public class LevelData
    {
        public int startingGold { get; set; }
        public int lives { get; set; }
        public string[] levelLayout { get; set; }
        public int startPosX { get; set; }
        public int startPosY { get; set; }
        public List<WaveData> Waves { get; set; }
    }

    public class WaveData
    {
        public List<EnemyData> Enemies { get; set; }
        public int waveReward { get; set; }  
    }

    public class EnemyData
    {
        public int numberOfTheseEnemies { get; set; }
        public int speed { get; set; }
        public int health { get; set; }
        public int damage { get; set; }
        public int reward { get; set; }
        public string colorName { get; set; }
        [JsonIgnore]
        public Color color //JSON doesn't know Color.[name of a color] so we have to convert the color name to a Color object
        {
            get
            {
                return Color.FromName(colorName);
            }
        }
    }

    public void LoadLevel(string filePath)
    {
        if (!File.Exists(filePath))
        {
            MessageBox.Show("Level file not found!");
            return;
        }

        string jsonString = File.ReadAllText(filePath); // Read the JSON file content as a string
        CurrentLevel = JsonSerializer.Deserialize<LevelData>(jsonString); // Deserialize the JSON string into a LevelData object

    }
}

