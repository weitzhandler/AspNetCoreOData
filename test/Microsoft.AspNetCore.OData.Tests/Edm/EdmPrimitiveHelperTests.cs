﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.OData.Common;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.TestCommon;
using Microsoft.AspNetCore.OData.Tests.Commons;
using Xunit;

namespace Microsoft.AspNetCore.OData.Tests.Edm
{
    public class EdmPrimitiveHelperTests
    {
        public static TheoryDataSet<object, object, Type> ConvertPrimitiveValue_NonStandardPrimitives_Data
        {
            get
            {
                return new TheoryDataSet<object, object, Type>
                {
                     { "1", (char)'1', typeof(char) },
                     { "1", (char?)'1', typeof(char?) },
                     { "123", (char[]) new char[] {'1', '2', '3' }, typeof(char[]) },
                     { (int)1 , (ushort)1, typeof(ushort)},
                     { (int?)1, (ushort?)1,  typeof(ushort?) },
                     { (long)1, (uint)1,  typeof(uint) },
                     { (long?)1, (uint?)1, typeof(uint?) },
                     { (long)1 , (ulong)1, typeof(ulong)},
                     { (long?)1 ,(ulong?)1, typeof(ulong?)},
                    //(Stream) new MemoryStream(new byte[] { 1 }), // TODO: Enable once we have support for streams
                     { "<element xmlns=\"namespace\" />" ,(XElement)new XElement(XName.Get("element","namespace")), typeof(XElement)},
                };
            }
        }

        public static TheoryDataSet<DateTimeOffset> ConvertDateTime_NonStandardPrimitives_Data
        {
            get
            {
                return new TheoryDataSet<DateTimeOffset>
                {
                    DateTimeOffset.Parse("2014-12-12T01:02:03Z"),
                    DateTimeOffset.Parse("2014-12-12T01:02:03-8:00"),
                    DateTimeOffset.Parse("2014-12-12T01:02:03+8:00"),
                };
            }
        }

        [Theory]
        [MemberData(nameof(ConvertPrimitiveValue_NonStandardPrimitives_Data))]
        public void ConvertPrimitiveValue_NonStandardPrimitives(object valueToConvert, object result, Type conversionType)
        {
            // Arrange & Act
            object actual = EdmPrimitiveHelper.ConvertPrimitiveValue(valueToConvert, conversionType);

            // Assert
            Assert.Equal(result.GetType(), actual.GetType());
            Assert.Equal(result.ToString(), actual.ToString());
        }

        [Theory]
        [MemberData(nameof(ConvertDateTime_NonStandardPrimitives_Data))]
        public void ConvertDateTimeValue_NonStandardPrimitives_DefaultTimeZoneInfo(DateTimeOffset valueToConvert)
        { 
            // Arrange & Act
            object actual = EdmPrimitiveHelper.ConvertPrimitiveValue(valueToConvert, typeof(DateTime));

            // Assert
            DateTime dt = Assert.IsType<DateTime>(actual);
            Assert.Equal(valueToConvert.LocalDateTime, dt);
        }

        [Theory]
        [MemberData(nameof(ConvertDateTime_NonStandardPrimitives_Data))]
        public void ConvertDateTimeValue_NonStandardPrimitives_CustomTimeZoneInfo(DateTimeOffset valueToConvert)
        {
            // Arrange & Act
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            object actual = EdmPrimitiveHelper.ConvertPrimitiveValue(valueToConvert, typeof(DateTime), timeZone);

            // Assert
            DateTime dt = Assert.IsType<DateTime>(actual);
            Assert.Equal(TimeZoneInfo.ConvertTime(valueToConvert, timeZone).DateTime, dt);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("")]
        public void ConvertPrimitiveValueToChar_Throws(string input)
        {
            // Arrange & Act & Assert
            ExceptionAssert.Throws<ValidationException>(
                () => EdmPrimitiveHelper.ConvertPrimitiveValue(input, typeof(char)),
                "The value must be a string with a length of 1.");
        }

        [Fact]
        public void ConvertPrimitiveValueToNullableChar_Throws()
        {
            // Arrange & Act & Assert
            ExceptionAssert.Throws<ValidationException>(
                () => EdmPrimitiveHelper.ConvertPrimitiveValue("123", typeof(char?)),
                "The value must be a string with a maximum length of 1.");
        }

        [Fact]
        public void ConvertPrimitiveValueToXElement_Throws_IfInputIsNotString()
        {
            // Arrange & Act & Assert
            ExceptionAssert.Throws<ValidationException>(
                () => EdmPrimitiveHelper.ConvertPrimitiveValue(123, typeof(XElement)),
                "The value must be a string.");
        }
    }
}
