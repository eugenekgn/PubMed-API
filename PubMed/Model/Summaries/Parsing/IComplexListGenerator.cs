﻿using System;
using System.Linq;
using PubMed.Model.Summaries.Internal;

namespace PubMed.Model.Summaries.Parsing
{
    /// <summary>
    ///     Provides functionality to generate lists that aren't simple (e.g. for things such as Authors, History etc).
    /// </summary>
    public interface IComplexListGenerator
    {
        object GenerateList(eSummaryResultDocSumItem baseListItem);
    }

    public class ArticleIDsListGenerator : IComplexListGenerator
    {
        public object GenerateList(eSummaryResultDocSumItem baseListItem)
        {
            var articleIds = from eSummaryResultDocSumItemItem item in baseListItem.Item
                             select new ArticleID
                                    {
                                        IDKey = item.Name,
                                        IDValue = item.Value
                                    };
            return articleIds.ToList();
        }
    }

    public class HistoryListGenerator : IComplexListGenerator
    {
        public object GenerateList(eSummaryResultDocSumItem baseListItem)
        {
            var histories = from eSummaryResultDocSumItemItem item in baseListItem.Item
                            select new History
                                   {
                                       HistoryEntryType = item.Name,
                                       Date = ConvertTextToDateTime(item.Value)
                                   };
            return histories.ToList();
        }

        private DateTime? ConvertTextToDateTime(string text)
        {
            DateTime time;
            var parsedSuccessfully = DateTime.TryParse(text, out time);
            if (parsedSuccessfully)
            {
                return time;
            }

            return null;
        }
    }

    public class AuthorsListGenerator : IComplexListGenerator
    {
        public object GenerateList(eSummaryResultDocSumItem baseListItem)
        {
            var authors = from item in baseListItem.Item
                          select new Author
                                 {
                                     Name = item.Value,
                                     Type = item.Name
                                 };
            return authors.ToList();
        }
    }
}