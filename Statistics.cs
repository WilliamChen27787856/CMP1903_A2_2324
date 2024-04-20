
using System;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public class Statistics {


    // Create a lazily-loaded Singleton instance of the Statistics class.
    private static readonly Lazy<Statistics> lazyStats = new Lazy<Statistics>(() => new Statistics());
    public static Statistics INSTANCE { get { return lazyStats.Value; } }

    private Dictionary<string, int> _highScores;
    private Dictionary<string, int> _gamesPlayed;
    private Dictionary<string, List<int[]>> _allGameRolls;

    private Statistics() {
      this._highScores = new Dictionary<string, int>();
      this._gamesPlayed = new Dictionary<string, int>();
      this._allGameRolls = new Dictionary<string, List<int[]>>();
    }

    public void AddNewGameRoll(Game game, int[] rolledDie) {
      DictionaryInit<List<int[]>>(game.GameName, new List<int[]>(), this._allGameRolls);
      this._allGameRolls[game.GameName].Add(rolledDie);
    }

    public void AddEndGameStats(Game game) {
      HighScore(game);
      AddGamePlayed(game);
    }

    private bool HighScore(Game game) {
      DictionaryInit<int>(game.GameName, 0, this._highScores);
      int highestScore = Math.Max(game.PlayerOneScore, game.PlayerTwoScore);

      if (highestScore <= this._highScores[game.GameName]) {
        return false;
      }

      this._highScores[game.GameName] = highestScore;
      return true;
    }

    private void AddGamePlayed(Game game) {
      DictionaryInit<int>(game.GameName, 0, this._gamesPlayed);
      this._gamesPlayed[game.GameName]++;
    }

    public static void DictionaryInit<T>(string name, T initialValue, Dictionary<string, T> dict) {
      if (dict.ContainsKey(name)) {
        return;
      }

      dict.Add(name, initialValue);
    }

  }

}
