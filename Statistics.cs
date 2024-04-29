using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public class Statistics {


    // Create a lazily-loaded Singleton instance of the Statistics class.
    private static readonly Lazy<Statistics> lazyStats = new Lazy<Statistics>(() => new Statistics());
    public static Statistics INSTANCE { get { return lazyStats.Value; } }

    private HashSet<string> _gameNames;
    private Dictionary<string, int> _highScores;
    private Dictionary<string, int> _gamesPlayed;
    private Dictionary<string, List<int[]>> _allGameRolls;

    private Statistics() {
      this._gameNames = new HashSet<string>();
      this._highScores = new Dictionary<string, int>();
      this._gamesPlayed = new Dictionary<string, int>();
      this._allGameRolls = new Dictionary<string, List<int[]>>();
    }

    public void AddNewGameRoll(Game game, int[] rolledDie) {
      this.CheckGameExists(game.GameName);
      this._allGameRolls[game.GameName].Add(rolledDie);
    }

    public void AddEndGameStats(Game game) {
      this.CheckGameExists(game.GameName);
      HighScore(game);
      AddGamePlayed(game);
    }

    private bool HighScore(Game game) {
      int highestScore = Math.Max(game.PlayerOneScore, game.PlayerTwoScore);

      if (highestScore <= this._highScores[game.GameName]) {
        return false;
      }

      this._highScores[game.GameName] = highestScore;
      return true;
    }

    private void AddGamePlayed(Game game) {
      this._gamesPlayed[game.GameName]++;
    }

    private void CheckGameExists(string name) {
      if (this._gameNames.Contains(name)) {
        return;
      }

      this._gameNames.Add(name);
      this._highScores.Add(name, 0);
      this._gamesPlayed.Add(name, 0);
      this._allGameRolls.Add(name, new List<int[]>());
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      foreach (string gameName in this._gameNames) {
        sb.AppendLine();
        sb.AppendLine($"============= {gameName} =============");
        sb.AppendLine($"High Score: {this._highScores[gameName]}");
        sb.AppendLine($"Games Played: {this._gamesPlayed[gameName]}");
        // TODO: Add statistics.
        sb.AppendLine($"=============={new string('=', gameName.Length)}==============");
        sb.AppendLine();
      }
      return sb.ToString();
    }
  }

}
