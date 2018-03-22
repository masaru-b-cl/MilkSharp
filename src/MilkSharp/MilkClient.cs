using System;

namespace MilkSharp
{
    public class MilkClient
    {
        public MilkContext Context { get; }

        private Lazy<MilkTest> test;
        private Lazy<MilkAuth> auth;
        private Lazy<MilkLists> lists;
        private Lazy<MilkTasks> tasks;

        public MilkClient(MilkContext context)
        {
            Context = context;

            test = new Lazy<MilkTest>(() => new MilkTest(context));
            auth = new Lazy<MilkAuth>(() => new MilkAuth(context));
            lists = new Lazy<MilkLists>(() => new MilkLists(context));
            tasks = new Lazy<MilkTasks>(() => new MilkTasks(context));
        }
        
        internal MilkClient(IMilkRawClient rawClient)
        {
            auth = new Lazy<MilkAuth>(() => new MilkAuth(rawClient));
            lists = new Lazy<MilkLists>(() => new MilkLists(rawClient));
            test = new Lazy<MilkTest>(() => new MilkTest(rawClient));
            tasks = new Lazy<MilkTasks>(() => new MilkTasks(rawClient));
        }

        public MilkAuth Auth => auth.Value;
        public MilkLists Lists => lists.Value;
        public MilkTest Test => test.Value;
        public MilkTasks Tasks => tasks.Value;
    }
}