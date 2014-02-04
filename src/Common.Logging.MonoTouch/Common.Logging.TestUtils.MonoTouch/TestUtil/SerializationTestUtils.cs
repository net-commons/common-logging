#region License

/*
 * Copyright 2002-2007 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Common.Logging
{
    /// <summary>
    /// Utilities for testing serializability of objects.
    /// </summary>
    /// <remarks>
    /// Exposes static methods for use in other test cases.
    /// </remarks>
    /// <author>Erich Eichinger</author>
    [TestFixture]
    public sealed class SerializationTestUtils
    {
        #region Test Ourselves

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void WithNonSerializableObject()
        {
            TestObject o = new TestObject();
            Assert.IsFalse(o is ISerializable);
            Assert.IsFalse(IsSerializable(o));
            TrySerialization(o);
        }

        [Test]
        public void WithSerializableObject()
        {
            SerializableTestObject pA = new SerializableTestObject("propA");
            Assert.IsTrue(IsSerializable(pA));
            TrySerialization(pA);
            SerializableTestObject pB = SerializeAndDeserialize(pA);
            Assert.IsFalse(ReferenceEquals(pA, pB));
            Assert.AreEqual(pA.SomeProperty, pB.SomeProperty);
        }

        #endregion

        /// <summary>
        /// Attempts to serialize the specified object to an in-memory stream.
        /// </summary>
        /// <param name="o">the object to serialize</param>
        public static void TrySerialization(object o)
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, o);
            }
        }

        /// <summary>
        /// Tests whether the specified object is serializable.
        /// </summary>
        /// <param name="o">the object to test.</param>
        /// <returns>true if the object is serializable, otherwise false.</returns>
        public static bool IsSerializable(object o)
        {
            return o == null ? true : o.GetType().IsSerializable;
        }

        /// <summary>
        /// Tests whether instances of the specified type are serializable.
        /// </summary>
        /// <returns>true if the type is serializable, otherwise false.</returns>
        public static bool IsSerializable<T>()
        {
            return typeof(T).IsSerializable;
        }

        /// <summary>
        /// Serializes the specified object to an in-memory stream, and returns
        /// the result of deserializing the object stream.
        /// </summary>
        /// <param name="o">the object to use.</param>
        /// <returns>the deserialized object.</returns>
        public static T SerializeAndDeserialize<T>(T o)
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, o);
                stream.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                T o2 = (T)bformatter.Deserialize(stream);
                return o2;
            }
        }

        #region Test Classes

        private class TestObject
        {
        }

        [Serializable]
        private class SerializableTestObject
        {
            public readonly string SomeProperty;

            public SerializableTestObject(string someProperty)
            {
                SomeProperty = someProperty;
            }
        }

        #endregion
    }
}