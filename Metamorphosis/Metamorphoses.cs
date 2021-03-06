﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamorphosis
{
    public class Metamorphoses : IMetamorphoses
    {
        public IEnumerable<Item> CutMinus(IEnumerable<Item> t)
        {
            return t.Where(w => w.Quantity < 0 || w.Remains < 0).ToList();
        }
        public IEnumerable<Item> GetRemains(IEnumerable<Item> lm)
        {
            decimal d = lm
                .Select(s => s.Quantity)
                .Sum();
            foreach (var v in lm)
            {
                //to prevent trailing zeros
                v.Rest = (decimal)Math.Round(d, 5) / 1.000000000000000000000000000000000m;
                d -= v.Quantity;
                v.Remains = (decimal)Math.Round(v.Remains, 5) / 1.000000000000000000000000000000000m;
                v.Quantity = (decimal)Math.Round(v.Quantity, 5) / 1.000000000000000000000000000000000m;
            }
            return lm;
        }
        public IEnumerable<Item> CutDates(IEnumerable<Item> t, DateTime max)
        {
            //here day comes tomorrow
            max += TimeSpan.FromDays(1);
            return t
                .Where(w => w.Date < max)
                .ToList();
        }
        public IEnumerable<Item> CutDates(IEnumerable<Item> t, DateTime min, DateTime max)
        {
            max += TimeSpan.FromDays(1);
            return t
                .Where(w => w.Date >= min && w.Date < max)
                .ToList();
        }
        public IEnumerable<Item> Grouping(IEnumerable<Item> t, bool party, bool order, bool task, bool stat)
        {
            return t
                .GroupBy(
                    g => new
                    {
                        OidStore = g.OidStore,
                        OidUnit = g.OidUnit,
                        Party = party ? g.Party : null,
                        OrderRP = order ? g.OrderRP : null,
                        Task = task ? g.Task : null,
                        Stat = stat ? g.Stat : null
                    })
                .Select(
                    s => new Item
                    {
                        OidUnit = s.Key.OidUnit,
                        OidStore = s.Key.OidStore,
                        Party = party ? s.Key.Party : s.FirstOrDefault().Party,
                        OrderRP = order ? s.Key.OrderRP : s.FirstOrDefault().OrderRP,
                        Task = task ? s.Key.Task : s.FirstOrDefault().Task,
                        Stat = stat ? s.Key.Stat : s.FirstOrDefault().Stat,
                        ComtecNumber = s.FirstOrDefault().ComtecNumber,
                        Date = s.FirstOrDefault().Date,
                        DateString = s.FirstOrDefault().DateString,
                        DocumentName = s.FirstOrDefault().DocumentName,
                        DocumentNumber = s.FirstOrDefault().DocumentNumber,
                        DocumentType = s.FirstOrDefault().DocumentType,
                        Group = s.FirstOrDefault().Group,
                        KeyArticle = s.FirstOrDefault().KeyArticle,
                        Movement = s.FirstOrDefault().Movement,
                        Refill = s.FirstOrDefault().Refill,
                        StoreCell = s.FirstOrDefault().StoreCell,
                        StoreKey = s.FirstOrDefault().StoreKey,
                        StoreString = s.FirstOrDefault().StoreString,
                        StroreName = s.FirstOrDefault().StroreName,
                        UnitMeasurement = s.FirstOrDefault().UnitMeasurement,
                        UnitName = s.FirstOrDefault().UnitName,
                        User = s.FirstOrDefault().User,
                        Transition = s.FirstOrDefault().Transition,
                        Ready = s.FirstOrDefault().Ready,
                        NextOperation = s.FirstOrDefault().NextOperation,
                        Quantity = (decimal)Math.Round((s.Sum(ss => ss.Quantity)), 5) / 1.000000000000000000000000000000000m,
                        Remains = (decimal)Math.Round((s.Sum(ss => ss.Remains)), 5) / 1.000000000000000000000000000000000m
                    })
                .ToList();
        }
        public IEnumerable<Item> RenameCells(IEnumerable<Item> items, Guid guid, string newCell)
        {
            //check perfomance
            //foreach (var item in items)
            //{
            //    if (item.OidUnit == guid) 
            //        item.StoreCell = newCell;
            //    yield return item;
            //}
            return items.Select(s => new Item
                {
                    ComtecNumber = s.ComtecNumber,
                    Date = s.Date,
                    DateString = s.DateString,
                    DocumentName = s.DocumentName,
                    DocumentNumber = s.DocumentNumber,
                    DocumentType = s.DocumentType,
                    Group = s.Group,
                    KeyArticle = s.KeyArticle,
                    Movement = s.Movement,
                    NextOperation = s.NextOperation,
                    OidStore = s.OidStore,
                    OidUnit = s.OidUnit,
                    OrderRP = s.OrderRP,
                    Party = s.Party,
                    Quantity = s.Quantity,
                    Ready = s.Ready,
                    Refill = s.Refill,
                    Remains = s.Remains,
                    Rest = s.Rest,
                    Stat = s.Stat,
                    StoreCell = s.OidUnit != guid ? s.StoreCell : newCell,
                    StoreKey = s.StoreKey,
                    StoreString = s.StoreString,
                    StroreName = s.StroreName,
                    Task = s.Task,
                    Transition = s.Transition,
                    UnitMeasurement = s.UnitMeasurement,
                    UnitName = s.UnitName,
                    User = s.User
                })
                .ToList();
        }
    }
}
