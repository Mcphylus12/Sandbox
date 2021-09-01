using System;
using System.Linq.Expressions;

namespace StubGenerator
{
    public static class ObjectGenerationExtensions
    {
        public static Generator<T> AddDefaultGenerators<T>(this Generator<T> generator)
        {
            return generator
                .AddGenerator(new RandomStringGenerator(10))
                .AddGenerator(new RandomCharGenerator())
                .AddGenerator(new RandomDoubleGenerator(0, 50))
                .AddGenerator(new RandomDateGenerator(20))
                .AddGenerator(new RandomBoolGenerator());
        }

        public static Generator<T> AddValue<T, TValue>(this Generator<T> generator, TValue value)
        {
            return generator
                .AddGenerator(new SimpleValueGenerator<TValue>(value));
        }

        public static Generator<T> AddGenerator<T, TValue>(this Generator<T> generator, Func<TValue> valueGenerator)
        {
            return generator
                .AddGenerator(new SimpleValueGenerator<TValue>(valueGenerator));
        }

        public static Generator<T> AddValue<T, TModel, TProp>(this Generator<T> generator, TProp value, params Expression<Func<TModel, TProp>>[] accessors)
        {
            return generator
                .AddGenerator(new SimpleValueGenerator<TProp>(value), accessors);
        }

        public static Generator<T> Ignore<T, TModel, TProp>(this Generator<T> generator, params Expression<Func<TModel, TProp>>[] accessors)
            where TProp : class
        {
            return generator.AddValue(null, accessors);
        }

        public static Generator<T> AddGenerator<T, TModel, TProp>(this Generator<T> generator, Func<TProp> valueGenerator, params Expression<Func<TModel, TProp>>[] accessors)
        {
            return generator
                .AddGenerator(new SimpleValueGenerator<TProp>(valueGenerator), accessors);
        }
    }
}
