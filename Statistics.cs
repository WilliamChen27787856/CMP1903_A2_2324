using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  /// <summary>
  /// A class that is used to store a standard set of statistics for all of the games.
  /// </summary>
  public class Statistics {

    /// <value>
    /// A private field that stores the Singleton instance contained within a Lazy object to allow
    /// for lazy-loading.
    /// </value>
    /// <remarks>
    /// Lazy loading will mean that we do not have to instantiate the class until the
    /// object is actually required.
    /// </remarks>
    private static readonly Lazy<Statistics> lazyStats =
      new Lazy<Statistics>(() => new Statistics());

    /// <value>
    /// A public static property to retrieve the Singleton instance of the Statistics.
    /// </value>
    public static Statistics INSTANCE { get { return lazyStats.Value; } }

    /// <value>
    /// A set used to store all the game identifiers that are valid within the Statistics class.
    /// </value>
    private HashSet<string> _gameNames;

    /// <value>
    /// A dictionary to get the high score for each game.
    /// </value>
    private Dictionary<string, int> _highScores;

    /// <value>
    /// A dictionary to get the number of games of each type played.
    /// </value>
    private Dictionary<string, int> _gamesPlayed;

    /// <value>
    /// A dictionary to get the rolls made in each game.
    /// </value>
    private Dictionary<string, List<int[]>> _allGameRolls;

    /// <value>
    /// A dictionary to get all the scores for each game.
    /// </value>
    private Dictionary<string, List<Tuple<int, int>>> _gameScores;

    /// <summary>
    /// A constructor to instantiate each of the objects required for the Statistics class to
    /// function properly.
    /// </summary>
    private Statistics() {
      this._gameNames = new HashSet<string>();
      this._highScores = new Dictionary<string, int>();
      this._gamesPlayed = new Dictionary<string, int>();
      this._allGameRolls = new Dictionary<string, List<int[]>>();
      this._gameScores = new Dictionary<string, List<Tuple<int, int>>>();
    }

    /// <summary>
    /// Adds an array of rolls for a game to the <c>_allGameRolls</c> method.
    /// </summary>
    /// <param name="game">
    /// A <c>Game</c> object for the game being played.
    /// </param>
    /// <param name="rolledDie">
    /// An array of integers that the game had rolled.
    /// </param>
    public void AddNewGameRoll(Game game, int[] rolledDie) {
      this.CheckGameExists(game.GameName);
      this._allGameRolls[game.GameName].Add(rolledDie);
    }

    /// <summary>
    /// Adds the final statistics for a game, this should be called after the game is over.
    /// </summary>
    /// <param name="game">
    /// A <c>Game</c> object for the game that was being played.
    /// </param>
    public void AddEndGameStats(Game game) {
      this.CheckGameExists(game.GameName);
      AddGameScore(game);
      HighScore(game);
      AddGamePlayed(game);
    }

    private void AddGameScore(Game game) {
      this._gameScores[game.GameName].Add(new Tuple<int, int> (game.PlayerOneScore, game.PlayerTwoScore));
    }

    /// <summary>
    /// Checks if the game is a new high-score.
    /// </summary>
    /// <param name="game">
    /// A <c>Game</c> object for the game that was being played.
    /// </param>
    /// <returns>
    /// A boolean value for if the high score was added successfully.
    /// </returns>
    private bool HighScore(Game game) {
      int highestScore = Math.Max(game.PlayerOneScore, game.PlayerTwoScore);

      if (highestScore <= this._highScores[game.GameName]) {
        return false;
      }

      this._highScores[game.GameName] = highestScore;
      return true;
    }

    /// <summary>
    /// Adds a new game played to the <c>Statistics</c> class.
    /// </summary>
    /// <param name="game">
    /// A <c>Game</c> object for the game that was being played.
    /// </param>
    private void AddGamePlayed(Game game) {
      this._gamesPlayed[game.GameName]++;
    }

    /// <summary>
    /// Checks if a game exists within the Statistics class, if not it will create entries for all
    /// of them.
    /// </summary>
    /// <param name="name">
    /// A string of the game's identifier.
    /// </param>
    private void CheckGameExists(string name) {
      if (this._gameNames.Contains(name)) {
        return;
      }

      this._gameNames.Add(name);
      this._highScores.Add(name, 0);
      this._gamesPlayed.Add(name, 0);
      this._allGameRolls.Add(name, new List<int[]>());
      this._gameScores.Add(name, new List<Tuple<int, int>>());
    }

    /// <summary>
    /// Overrides the default ToString method to return the statistics information.
    /// </summary>
    /// <returns>
    /// A string containing the statistics for each game.
    /// </returns>
    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      foreach (string gameName in this._gameNames) {
        sb.AppendLine();
        sb.AppendLine($"============= {gameName} =============");
        sb.AppendLine($"High Score: {this._highScores[gameName]}");
        sb.AppendLine($"Games Played: {this._gamesPlayed[gameName]}");
        sb.AppendLine($"Game Scores: ");

        int total = 0;
        foreach (Tuple<int, int> scoreTuple in this._gameScores[gameName]) {
          sb.AppendLine($"\tP1: {scoreTuple.Item1}\t| P2: {scoreTuple.Item2}");
          total += scoreTuple.Item1;
          total += scoreTuple.Item2;
        }
        sb.AppendLine($"Average Score: {total / (this._gameScores[gameName].Count * 2)}");

        sb.AppendLine($"=============={new string('=', gameName.Length)}==============");
        sb.AppendLine();
      }
      return sb.ToString();
    }
  }

}
