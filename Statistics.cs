
using System;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public class Statistics {

    private Dictionary<string, int> _highScores;
    private Dictionary<string, int> _gamesPlayed;

    public Statistics() {
      this._highScores = new();
      this._gamesPlayed = new();
    }

    public void AddPlayedGame(Game game) {
      // TODO: Add each roll group while the game is playing, rather than at the end.
      HighScore(game);
      AddGamePlayed(game);
    }

    public bool HighScore(Game game) {
      DictionaryInit<int>(game.GameName, 0, this._highScores);
      int highestScore = Math.Max(game.PlayerOneMove, game.PlayerTwoScore);

      if (highestScore <= this._highScores[game.GameName]) {
        return false;
      }

      this._highScores[game.GameName] = highestScore;
      return true;
    }

    public void AddGamePlayed(Game game) {
      DictionaryInit<int>(game.GameName, 0, this._gamesPlayed);
      this._gamesPlayed[game.GameName]++;
    }

    public static void DictionaryInit<V>(string name, T initialValue, Dictionary<string, T> dict) {
      if (dict.Contains(name)) {
        return;
      }

      dict.Add(name, initialValue);
    }

  }

}
