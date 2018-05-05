using System;

namespace MAL.NetLogic.Classes
{
    public interface ISeasonLookup
    {
        /// <summary>
        /// Convert a date to a season
        /// </summary>
        /// <param name="date">The Date to convert</param>
        /// <returns>The season as a SeasonEnum</returns>
        string CalculateCurrentSeason(DateTime date);

        /// <summary>
        /// Get the season following the one provided
        /// </summary>
        /// <param name="currentSeason">One of the seasons</param>
        /// <returns>The next season</returns>
        string GetNextSeason(string currentSeason);

        /// <summary>
        /// Given the current season and its year get the year for the following season
        /// </summary>
        /// <param name="currentSeason">One of the seasons</param>
        /// <param name="year">The year of the season</param>
        /// <returns>Year for the next season</returns>
        int NextSeasonYear(string currentSeason, int year);

        /// <summary>
        /// Get the start date of the season for a spesific date
        /// </summary>
        /// <param name="date">The date to lookup</param>
        /// <returns>The date on which the season started</returns>
        DateTime SeasonStart(DateTime date);

        /// <summary>
        /// Get the end date of the season for a spesific date
        /// </summary>
        /// <param name="date">The date to lookup</param>
        /// <returns>The date on which the season ended</returns>
        DateTime SeasonEnd(DateTime date);
    }
}