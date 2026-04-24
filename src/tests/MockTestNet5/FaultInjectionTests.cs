// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-04-24 09:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 09:55
// ***********************************************************************
//  <copyright file="FaultInjectionTests.cs" company="RzR SOFT & TECH">
//   Copyright � RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.EntityMock;

#endregion

namespace MockTestNet5
{
    [TestClass]
    public class FaultInjectionTests
    {
        [TestMethod]
        public async Task WithDelay_ApproximatelyHonored()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .WithDelay(TimeSpan.FromMilliseconds(50))
                .Build();

            var start = Environment.TickCount;
            var list = await seq.ToListAsync();
            var elapsedMs = Environment.TickCount - start;

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
            // 3 yielded items + 1 final MoveNext(false) = 4 delays
            Assert.IsTrue(elapsedMs >= 150, $"Expected >= 150ms, got {elapsedMs}ms.");
        }

        [TestMethod]
        public async Task ThrowAfter_ThrowsAtConfiguredIndex()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 10, 20, 30, 40 })
                .ThrowAfter(2, new InvalidOperationException("boom"))
                .Build();

            var collected = 0;
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq)
                    collected++;
            });

            Assert.AreEqual("boom", ex.Message);
            Assert.AreEqual(2, collected);
        }

        [TestMethod]
        public async Task ThrowAfter_FactoryReceivesIndex()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .ThrowAfter(1, i => new InvalidOperationException($"idx={i}"))
                .Build();

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq)
                {
                }
            });

            Assert.AreEqual("idx=1", ex.Message);
        }

        [TestMethod]
        public async Task CancelAfter_CancelsIteration()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(Enumerable.Range(1, 100).ToArray())
                .WithDelay(TimeSpan.FromMilliseconds(20))
                .CancelAfter(TimeSpan.FromMilliseconds(60))
                .Build();

            Exception caught = null;
            try
            {
                await foreach (var _ in seq)
                {
                }
            }
            catch (OperationCanceledException ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught, "Expected OperationCanceledException.");
        }

        [TestMethod]
        public async Task NoFaults_BehavesAsPlainSequence()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .Build();

            var list = await seq.ToListAsync();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [TestMethod]
        public async Task ExternalCancellation_StillObserved()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(Enumerable.Range(1, 100).ToArray())
                .WithDelay(TimeSpan.FromMilliseconds(30))
                .Build();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(80));

            Exception caught = null;
            try
            {
                await foreach (var _ in seq.WithCancellation(cts.Token))
                {
                }
            }
            catch (OperationCanceledException ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught, "Expected OperationCanceledException.");
        }

        [TestMethod]
        public void WithDelay_NegativeThrows()
            => Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                AsyncEnumerableFactory.Builder<int>().WithDelay(TimeSpan.FromMilliseconds(-1)));

        [TestMethod]
        public void CancelAfter_NegativeThrows()
            => Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                AsyncEnumerableFactory.Builder<int>().CancelAfter(TimeSpan.FromMilliseconds(-1)));

        [TestMethod]
        public void ThrowAfter_NegativeIndexThrows()
            => Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                AsyncEnumerableFactory.Builder<int>().ThrowAfter(-1, new Exception()));

        [TestMethod]
        public void ThrowAfter_NullExceptionThrows()
            => Assert.ThrowsException<ArgumentNullException>(() =>
                AsyncEnumerableFactory.Builder<int>().ThrowAfter(0, (Exception)null));

        [TestMethod]
        public void ThrowAfter_NullFactoryThrows()
            => Assert.ThrowsException<ArgumentNullException>(() =>
                AsyncEnumerableFactory.Builder<int>().ThrowAfter(0, (Func<int, Exception>)null));

        [TestMethod]
        public async Task ThrowAfter_AtIndexZero_ThrowsBeforeAnyItem()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .ThrowAfter(0, new InvalidOperationException("immediate"))
                .Build();

            var collected = 0;
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq) collected++;
            });

            Assert.AreEqual(0, collected);
        }

        [TestMethod]
        public async Task ThrowAfter_BeyondLength_NeverThrows()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .ThrowAfter(99, new InvalidOperationException("never"))
                .Build();

            var list = await seq.ToListAsync();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [TestMethod]
        public async Task DefaultExceptionFactory_UsedWhenNoneProvided()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2 })
                .ThrowAfter(1, i => new InvalidOperationException($"at {i}"))
                .Build();

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq) { }
            });
            Assert.AreEqual("at 1", ex.Message);
        }

        [TestMethod]
        public async Task EmptySequence_WithFaults_CompletesImmediately()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .WithDelay(TimeSpan.FromMilliseconds(5))
                .Build();

            var list = await seq.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public async Task PreCancelledToken_ThrowsImmediately()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .Build();

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Exception caught = null;
            try
            {
                await foreach (var _ in seq.WithCancellation(cts.Token)) { }
            }
            catch (OperationCanceledException ex) { caught = ex; }

            Assert.IsNotNull(caught);
        }

        [TestMethod]
        public async Task CancelAfter_WithoutDelay_StillCancelsLongLoop()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(Enumerable.Range(1, 1000).ToArray())
                .CancelAfter(TimeSpan.FromMilliseconds(10))
                .Build();

            Exception caught = null;
            try
            {
                await foreach (var _ in seq)
                {
                    await Task.Delay(5);
                }
            }
            catch (OperationCanceledException ex) { caught = ex; }

            Assert.IsNotNull(caught);
        }

        [TestMethod]
        public async Task DelayPlusThrow_DelayAppliedBeforeThrow()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .WithDelay(TimeSpan.FromMilliseconds(40))
                .ThrowAfter(2, new InvalidOperationException("late"))
                .Build();

            var start = Environment.TickCount;
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq) { }
            });
            var elapsedMs = Environment.TickCount - start;

            Assert.IsTrue(elapsedMs >= 100, $"Expected >= 100ms, got {elapsedMs}ms.");
        }

        [TestMethod]
        public async Task LinkedCancellation_CallerTokenWinsOverInternal()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(Enumerable.Range(1, 100).ToArray())
                .WithDelay(TimeSpan.FromMilliseconds(20))
                .CancelAfter(TimeSpan.FromSeconds(5))
                .Build();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(40));

            var start = Environment.TickCount;
            Exception caught = null;
            try
            {
                await foreach (var _ in seq.WithCancellation(cts.Token)) { }
            }
            catch (OperationCanceledException ex) { caught = ex; }
            var elapsedMs = Environment.TickCount - start;

            Assert.IsNotNull(caught);
            Assert.IsTrue(elapsedMs < 1000, $"Caller token should win: elapsed {elapsedMs}ms.");
        }

        [TestMethod]
        public async Task SameInstance_CanBeEnumeratedMultipleTimes()
        {
            var seq = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .ThrowAfter(2, new InvalidOperationException("x"))
                .Build();

            // First iteration throws after 2
            var first = 0;
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq) first++;
            });
            Assert.AreEqual(2, first);

            // Second iteration should also throw after 2 — independent enumerator state
            var second = 0;
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await foreach (var _ in seq) second++;
            });
            Assert.AreEqual(2, second);
        }

        [TestMethod]
        public async Task BuilderClear_ResetsFaultsAndItems()
        {
            var builder = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2, 3 })
                .ThrowAfter(1, new InvalidOperationException("x"));

            builder.Clear();
            builder.Add(42);

            var list = await builder.Build().ToListAsync();
            CollectionAssert.AreEqual(new[] { 42 }, list);
        }

        [TestMethod]
        public async Task BuildAfterAdd_DoesNotAffectAlreadyBuiltSequence()
        {
            var builder = AsyncEnumerableFactory.Builder<int>()
                .AddRange(new[] { 1, 2 });
            var built = builder.Build();

            builder.Add(99); // should not leak into 'built'

            var list = await built.ToListAsync();
            CollectionAssert.AreEqual(new[] { 1, 2 }, list);
        }
    }
}