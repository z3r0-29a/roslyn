﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class ConstructorDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<ConstructorDeclarationSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new ConstructorDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor1()
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|collapse:
    {
    }|}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor2()
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|collapse:
    {
    }                 |}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor3()
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|collapse:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor4()
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|collapse:
    {
    }|}|} /* .ctor */
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor5()
        {
            const string code = @"
class C
{
    {|hint:$$public C() // .ctor{|collapse:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor6()
        {
            const string code = @"
class C
{
    {|hint:$$public C() /* .ctor */{|collapse:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor7()
        {
            const string code = @"
class C
{
    {|hint:$$public C()
    // .ctor{|collapse:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructor8()
        {
            const string code = @"
class C
{
    {|hint:$$public C()
    /* .ctor */{|collapse:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("collapse", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructorWithComments()
        {
            const string code = @"
class C
{
    {|span1:// Foo
    // Bar|}
    {|hint2:$$public C(){|collapse2:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Foo ...", autoCollapse: true),
                Region("collapse2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestConstructorMissingCloseParenAndBody()
        {
            // Expected behavior is that the class should be outlined, but the constructor should not.

            const string code = @"
class C
{
    $$C(
}";

            await VerifyNoBlockSpansAsync(code);
        }
    }
}
