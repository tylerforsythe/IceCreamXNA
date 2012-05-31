#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endif

using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream
{
    ///// <summary>
    ///// A pre-allocated pool of objects with the ability to sort
    ///// and maintain objects as they become invalidated.
    ///// </summary>
    ///// <typeparam name="T">The type of object the pool will hold.</typeparam>
    //public class ResourcePool<T> where T : class
    //{
    //    #region Variables

    //    private ValidateObject objectCheck;
    //    private T[] objects;
    //    private int numberOfInvalidObjects;

    //    #endregion

    //    #region Properties

    //    /// <summary>
    //    /// Gets the maximum number of objects the pool can hold.
    //    /// </summary>
    //    public int MaximumSize
    //    {
    //        get { return objects.Length; }
    //    }

    //    /// <summary>
    //    /// Gets the number of invalid objects (open spaces) in the pool.
    //    /// </summary>
    //    public int NumberOfInvalidObjects
    //    {
    //        get { return numberOfInvalidObjects; }
    //    }

    //    /// <summary>
    //    /// Gets the number of valid objects (filled spaces) in the pool.
    //    /// </summary>
    //    public int NumberOfValidObjects
    //    {
    //        get { return objects.Length - numberOfInvalidObjects; }
    //    }

    //    /// <summary>
    //    /// Returns a valid object at the given index. Throws an exception
    //    /// if the index points to an invalid object space.
    //    /// </summary>
    //    /// <param name="index">The index of the valid object to get</param>
    //    /// <returns>A valid object found at the index</returns>
    //    public T this[int index]
    //    {
    //        get
    //        {
    //            index += NumberOfInvalidObjects;

    //            if (index < NumberOfInvalidObjects || index >= MaximumSize)
    //            {
    //                throw new IndexOutOfRangeException(
    //                "The index must be less than or equal to NumberOfValidObjects");
    //            }

    //            return objects[index];
    //        }
    //    }

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    /// Create a new pool with enough space for the allocated number of objects
    //    /// </summary>
    //    /// <param name="allocation">The number of objects to allocate space for</param>
    //    /// <param name="objectCheck">The method used to validate objects</param>
    //    public ResourcePool(int allocation, ValidateObject objectCheck)
    //        : this(allocation, objectCheck, null)
    //    { }

    //    /// <summary>
    //    /// Create a new pool with enough space for the allocated number of objects
    //    /// </summary>
    //    /// <param name="allocation">The number of objects to allocate space for</param>
    //    /// <param name="objectCheck">The method used to validate objects</param>
    //    public ResourcePool(int allocation, ValidateObject objectCheck, CreateNewObject objectCreate)
    //    {
    //        if (objectCheck == null)
    //            throw new Exception("ValidateObject delegate cannot be null.");

    //        this.objectCheck = objectCheck;

    //        numberOfInvalidObjects = allocation;

    //        objects = new T[allocation];
    //        for (int i = 0; i < allocation; i++)
    //        {
    //            if (objectCreate != null)
    //            {
    //                T t = objectCreate();

    //                if (t == null)
    //                    throw new Exception("CreateNewObject delegate cannot return null.");

    //                objects[i] = t;
    //            }
    //            else if (typeof(T).GetConstructor(null) != null)
    //                objects[i] = (T)typeof(T).GetConstructor(null).Invoke(null);
    //            else
    //                throw new Exception(
    //                "CreateNewObject delegate must be specified or T (" + typeof(T).ToString() + ") " +
    //                "must implement a parameterless constructor.");
    //        }
    //    }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Creates a new valid object to fill the spot of an invalidated
    //    /// object if possible. Returns null if all objects are valid.
    //    /// </summary>
    //    /// <returns>A reference to the next valid object, or null if all space is being used.</returns>
    //    public T GetNewValidObject()
    //    {
    //        if (numberOfInvalidObjects > 0)
    //            return objects[--numberOfInvalidObjects];

    //        return null;
    //    }

    //    /// <summary>
    //    /// Iterates through the objects checking for valid and
    //    /// invalid objects, arranging the internal list as needed.
    //    /// This method should be called each frame.
    //    /// </summary>
    //    public void ValidateObjects()
    //    {
    //        for (int i = numberOfInvalidObjects; i < objects.Length; i++)
    //        {
    //            T o = objects[i];

    //            if (!objectCheck(o))
    //            {
    //                if (i != numberOfInvalidObjects)
    //                {
    //                    objects[i] = objects[numberOfInvalidObjects];
    //                    objects[numberOfInvalidObjects] = o;
    //                }

    //                numberOfInvalidObjects++;
    //            }
    //        }
    //    }

    //    #endregion

    //    #region Delegate Declarations

    //    /// <summary>
    //    /// Checks if an object should be flagged as
    //    /// invalidated, allowing it's space in the pool
    //    /// to be given to another object.
    //    /// </summary>
    //    /// <param name="obj">The object to check</param>
    //    /// <returns>True if the object should be kept; false otherwise</returns>
    //    public delegate bool ValidateObject(T obj);

    //    /// <summary>
    //    /// Creates a new object during initialization.
    //    /// </summary>
    //    /// <returns>The newly created object. Cannot return null.</returns>
    //    public delegate T CreateNewObject();

    //    #endregion
    //}

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A pre-allocated pool of objects with the ability to sort
    /// and maintain objects as they become invalidated.
    /// </summary>
    /// <typeparam name="T">The type of object the pool will hold.</typeparam>
    public class ResourcePool<T> : IEnumerable<T>, IEnumerable where T: class
    {
        private const string CantCreateObjectExceptionFormat = "CreateNewObject delegate must be specified or T ({0}) must implement a parameterless constructor.";
        private const string CreateNewObjectReturnedNullException = "CreateNewObject delegate cannot return null.";
        private readonly ValidateObject objectCheck;
        private readonly T[] objects;

        public ResourcePool(int allocation, ValidateObject objectCheck) : this(allocation, objectCheck, null)
        {
        }

        public ResourcePool(int allocation, ValidateObject objectCheck, CreateNewObject objectCreate)
        {
            if (objectCheck == null)
            {
                throw new ArgumentNullException("objectCheck");
            }
            this.objectCheck = objectCheck;
            this.NumberOfInvalidObjects = allocation;
            this.objects = new T[allocation];
            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[0]);
            for (int i = 0; i < allocation; i++)
            {
                if (objectCreate != null)
                {
                    T t = objectCreate();
                    if (t == null)
                    {
                        throw new Exception("CreateNewObject delegate cannot return null.");
                    }
                    this.objects[i] = t;
                }
                else
                {
                    if (constructor == null)
                    {
                        throw new Exception(string.Format("CreateNewObject delegate must be specified or T ({0}) must implement a parameterless constructor.", typeof(T)));
                    }
                    this.objects[i] = (T) constructor.Invoke((object[]) null);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>) this.objects.GetEnumerator();
        }

        /// <summary>
        /// Creates a new valid object to fill the spot of an invalidated
        /// object if possible. Returns null if all objects are valid.
        /// </summary>
        /// <returns>A reference to the next valid object, or null if all space is being used.</returns>
        public T GetNewValidObject()
        {
             if (NumberOfInvalidObjects > 0)
                return objects[--NumberOfInvalidObjects];

            return default(T);            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.objects.GetEnumerator();
        }

        /// <summary>
        /// Iterates through the objects checking for valid and
        /// invalid objects, arranging the internal list as needed.
        /// This method should be called each frame.
        /// </summary>
        public void ValidateObjects()
        {
            for (int i = this.NumberOfInvalidObjects; i < this.objects.Length; i++)
            {
                T o = this.objects[i];
                if (!this.objectCheck(o))
                {
                    if (i != this.NumberOfInvalidObjects)
                    {
                        this.objects[i] = this.objects[this.NumberOfInvalidObjects];
                        this.objects[this.NumberOfInvalidObjects] = o;
                    }
                    this.NumberOfInvalidObjects++;
                }
            }
        }

        /// <summary>
        /// Returns a valid object at the given index. Throws an exception
        /// if the index points to an invalid object space.
        /// </summary>
        /// <param name="index">The index of the valid object to get</param>
        /// <returns>A valid object found at the index</returns>
        public T this[int index]
        {
            get
            {
                index += this.NumberOfInvalidObjects;
                if ((index < this.NumberOfInvalidObjects) || (index >= this.MaximumSize))
                {
                    throw new IndexOutOfRangeException("The index must be less than or equal to NumberOfValidObjects");
                }
                return this.objects[index];
            }
        }

        /// <summary>
        /// Gets the maximum number of objects the pool can hold.
        /// </summary>
        public int MaximumSize
        {
            get
            {
                return this.objects.Length;
            }
        }

        /// <summary>
        /// Gets the number of invalid objects (open spaces) in the pool.
        /// </summary>
        public int NumberOfInvalidObjects{get;set;}

        /// <summary>
        /// Gets the number of valid objects (filled spaces) in the pool.
        /// </summary>
        public int NumberOfValidObjects
        {
            get
            {
                return (this.objects.Length - this.NumberOfInvalidObjects);
            }
        }

        /// <summary>
        /// Creates a new object during initialization.
        /// </summary>
        /// <returns>The newly created object. Cannot return null.</returns>
        public delegate T CreateNewObject();

        /// <summary>
        /// Checks if an object should be flagged as
        /// invalidated, allowing it's space in the pool
        /// to be given to another object.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>True if the object should be kept; false otherwise</returns>
        public delegate bool ValidateObject(T obj);
    }

}
