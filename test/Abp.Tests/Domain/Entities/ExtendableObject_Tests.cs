﻿using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Json;
using Shouldly;
using Xunit;

namespace Abp.Tests.Domain.Entities
{
    public class ExtendableObject_Tests
    {
        [Fact]
        public void Should_Set_And_Get_Primitive_Values()
        {
            var entity = new MyEntity();

            entity.SetData("Name", "John");
            entity.GetData<string>("Name").ShouldBe("John");

            entity.SetData("Length", 42424242);
            entity.GetData<int>("Length").ShouldBe(42424242);

            entity.SetData("Age", 42);
            Assert.Equal(42, entity.GetData<byte>("Age"));

            entity.SetData("BirthDate", new DateTime(2015, 05, 25, 13, 24, 00, DateTimeKind.Utc));
            Assert.Equal(new DateTime(2015, 05, 25, 13, 24, 00, DateTimeKind.Utc), entity.GetData<DateTime>("BirthDate"));
        }

        [Fact]
        public void Should_Set_And_Get_Complex_Values()
        {
            var entity = new MyEntity();

            var obj = new MyComplexType
            {
                Name = "John",
                Age = 42,
                Inner = new List<MyComplexTypeInner>
                {
                    new MyComplexTypeInner {Value1 = "A", Value2 = 2},
                    new MyComplexTypeInner {Value1 = "B", Value2 = null},
                    new MyComplexTypeInner {Value1 = null, Value2 = null},
                    null
                }
            };

            entity.SetData("ComplexData", obj);
            var obj2 = entity.GetData<MyComplexType>("ComplexData");

            obj.ToJsonString().ShouldBe(obj2.ToJsonString());

            entity.SetData("ComplexData", null);
            entity.GetData<MyComplexType>("ComplexData").ShouldBe(null);
        }

        [Fact]
        public void Should_Get_Default_If_Not_Present()
        {
            var entity = new MyEntity();
            entity.GetData<string>("Name").ShouldBe(null);
            entity.GetData<int>("Length").ShouldBe(0);
            entity.GetData<int?>("Length").ShouldBe(null);
            entity.GetData<DateTime>("BirthDate").ShouldBe(new DateTime());
            entity.GetData<DateTime?>("BirthDate").ShouldBe(null);
            Assert.Equal(0, entity.GetData<byte>("Age"));
            Assert.Equal(null, entity.GetData<byte?>("Age"));
        }

        private class MyEntity : Entity, IExtendableObject
        {
            public string ExtensionData { get; set; }
        }

        public class MyComplexType
        {
            public string Name { get; set; }
            public byte Age { get; set; }
            public List<MyComplexTypeInner> Inner { get; set; }
        }

        public class MyComplexTypeInner
        {
            public string Value1 { get; set; }
            public int? Value2 { get; set; }
        }
    }
}
