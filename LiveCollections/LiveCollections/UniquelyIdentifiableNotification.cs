﻿using System.Collections.Generic;
using SpecializedCollections;

namespace System.Collections.LiveCollections {
    public abstract class UniquelyIdentifiableNotification<TKey, TItem> {
        protected internal UniquelyIdentifiableNotification() { }

        public abstract UniquelyIdentifiableNotification<TKey, TResult> Cast<TResult>()
                where TResult : TItem;

        public sealed class Insert : UniquelyIdentifiableNotification<TKey, TItem> {
            public TItem Item { get; private set; }
            public TKey Key { get; private set; }
            public Insert(TKey key, TItem item) {
                Key = key;
                Item = item;
            }

            public override UniquelyIdentifiableNotification<TKey, TResult> Cast<TResult>() {
                return new UniquelyIdentifiableNotification<TKey, TResult>.Insert(Key, (TResult)Item);
            }
        }

        public sealed class Remove : UniquelyIdentifiableNotification<TKey, TItem> {
            public TKey Key { get; private set; }
            public Remove(TKey key) {
                Key = key;
            }

            public override UniquelyIdentifiableNotification<TKey, TResult> Cast<TResult>() {
                return new UniquelyIdentifiableNotification<TKey, TResult>.Remove(Key);
            }
        }

        public sealed class Clear : UniquelyIdentifiableNotification<TKey, TItem> {
            public override UniquelyIdentifiableNotification<TKey, TResult> Cast<TResult>() {
                return new UniquelyIdentifiableNotification<TKey, TResult>.Clear();
            }
        }
    }

    public abstract class Notification<T> {
        protected internal Notification() { }

        public class Error : Notification<T> {
            public Exception Exception { get; private set; }
            public Error(Exception ex) {
                Exception = ex;
            }
        }

        public class Change : Notification<T> {
            public IEnumerable<T> Changes { get; private set; }
            public Change(IEnumerable<T> changes) {
                Changes = changes;
            }
        }

        public class Message : Notification<T> {
            public string Text { get; private set; }
            public Message(string text) {
                Text = text;
            }
        }

        public class Completed : Notification<T> {
            public Completed() { }
        }
    }

    public abstract class NotificationWithProgress<T> : Notification<T> {
        protected internal NotificationWithProgress() { }

        public class Indeterminate : NotificationWithProgress<T> {
            public Indeterminate() { }
        }

        public class ProgressMaximum : NotificationWithProgress<T> {
            public double MaximumValue { get; private set; }
            public ProgressMaximum(double maximumValue) {
                MaximumValue = maximumValue;
            }
        }

        public class Progress : NotificationWithProgress<T> {
            public double ProgressValue { get; private set; }
            public Progress(double progressValue) {
                ProgressValue = progressValue;
            }
        }
    }

    public interface IObservableHost<T> {
        IObservable<T> GetObservable();
    }

    public interface IUniqueLiveCollection<TKey, TItem>
        : IObservableHost<Notification<UniquelyIdentifiableNotification<TKey, TItem>>> {
    }

    internal class UniqueLivePropertySelector<TKey, TSource, TResult> : IUniqueLiveCollection<TKey, TResult>
        where TResult : IUniqueProperty<TKey, TResult> {

        private readonly Func<TSource, bool> _filter;
        private readonly Func<TSource, TResult> _map;
        private readonly InsertionOrderedDictionary<TKey, TResult> _items =
            new InsertionOrderedDictionary<TKey, TResult>();

        public IObservable<Notification<UniquelyIdentifiableNotification<TKey, TResult>>> GetObservable() {
            throw new NotImplementedException();
        }

        public UniqueLivePropertySelector(IObservable<Notification<UniquelyIdentifiableNotification<TKey, TResult>>> source, Func<TSource, bool> filter, Func<TSource, TResult> map) {
            _filter = filter;
            _map = map;
        }
    }

    public interface IUniqueProperty<TKey, TItem> : IObservableHost<TItem> {
    }

    public interface IUniqueProperty<TItem> : IObservableHost<TItem> {
    }

    public static class UniqueLiveCollections {
        //public static IUniqueProperty<R> Aggregate<TKey, T, R>(this IUniqueLiveCollection<TKey, T> source, Func<R, T> aggregator) {
        //
        //}
    }
}