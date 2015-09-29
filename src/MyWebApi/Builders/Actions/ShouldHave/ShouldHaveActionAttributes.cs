﻿// MyWebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.

namespace MyWebApi.Builders.Actions.ShouldHave
{
    using System;
    using System.Linq;
    using Common.Extensions;
    using Contracts.And;
    using Contracts.Attributes;
    using Exceptions;

    public partial class ShouldHaveTestBuilder<TActionResult>
    {
        public IAndTestBuilder<TActionResult> NoActionAttributes()
        {
            if (this.ActionLevelAttributes.Any())
            {
                throw new AttributeAssertionException(string.Format(
                    "When calling {0} action in {1} expected action to not have any action attributes, but in had some.",
                    this.ActionName,
                    this.Controller.GetName()));
            }

            return this.NewAndTestBuilder();
        }

        public IAndTestBuilder<TActionResult> ActionAttributes(int? withTotalNumberOf = null)
        {
            if (!this.ActionLevelAttributes.Any())
            {
                throw new AttributeAssertionException(string.Format(
                    "When calling {0} action in {1} expected action to have at least 1 action attribute, but in fact none was found.",
                    this.ActionName,
                    this.Controller.GetName()));
            }

            var actualNumberOfActionAttributes = this.ActionLevelAttributes.Count();
            if (withTotalNumberOf.HasValue && actualNumberOfActionAttributes != withTotalNumberOf)
            {
                throw new AttributeAssertionException(string.Format(
                    "When calling {0} action in {1} expected action to have {2} action {3}, but in fact found {4}.",
                    this.ActionName,
                    this.Controller.GetName(),
                    withTotalNumberOf,
                    withTotalNumberOf != 1 ? "attributes" : "attribute",
                    actualNumberOfActionAttributes));
            }

            return this.NewAndTestBuilder();
        }

        public IAndTestBuilder<TActionResult> ActionAttributes(Action<IAttributesTestBuilder> attributesTestBuilder)
        {
            this.ActionAttributes();
            var newAttributesTestBuilder = new AttributesTestBuilder(this.Controller, this.ActionName);
            attributesTestBuilder(newAttributesTestBuilder);
            var validations = newAttributesTestBuilder.GetAttributeValidations();
            validations.ForEach(v => v(this.ActionLevelAttributes));
            return this.NewAndTestBuilder();
        }
    }
}
