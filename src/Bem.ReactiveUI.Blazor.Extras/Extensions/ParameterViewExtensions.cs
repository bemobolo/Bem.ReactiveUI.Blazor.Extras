// --------------------------------------
// <copyright file="ParameterViewExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Internal;
using Bem.ReactiveUI.Blazor.Extras.Utils;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bem.ReactiveUI.Blazor.Extras.Extensions;

internal static class ParameterViewExtensions
{
    internal static readonly ParameterView Empty = (ParameterView)typeof(ParameterView)
        .GetField("_empty", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;

    private static readonly FieldInfo _ownerIndexField =
        typeof(ParameterView).GetField("_ownerIndex", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo _framesField =
        typeof(ParameterView).GetField("_frames", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly Func<ParameterView, int> _ownerIndex = ExpressionHelper.GetFieldValueExpression<ParameterView, int>(_ownerIndexField).Compile();

    private static readonly Func<ParameterView, RenderTreeFrame[]?> _frames = ExpressionHelper.GetFieldValueExpression<ParameterView, RenderTreeFrame[]>(_framesField).Compile();

    internal static ParameterView UpdateWith(this in ParameterView @this, in ParameterView newParameters)
    {
        var dict = (IDictionary<string, object?>)@this.ToDictionary();

        foreach (var parameter in newParameters)
        {
            dict[parameter.Name] = parameter.Value;
        }

        return ParameterView.FromDictionary(dict);
    }

    internal static (bool NotEqual, bool HasMutableParameters) Compare(this ParameterView @this, ParameterView oldParameters)
    {
        var hasMutableParameters = false;

        var oldIndex = oldParameters.GetOwnerIndex();
        var newIndex = @this.GetOwnerIndex();
        var oldEndIndexExcl = oldIndex + oldParameters.GetFrames()[oldIndex].ComponentSubtreeLength;
        var newEndIndexExcl = newIndex + @this.GetFrames()[newIndex].ComponentSubtreeLength;

        var equals = AreEqual();
        hasMutableParameters = FurtherCheckMutableParameters();

        bool AreEqual()
        {
            while (true)
            {
                // First, stop if we've reached the end of either subtree
                oldIndex++;
                newIndex++;
                var oldFinished = oldIndex == oldEndIndexExcl;
                var newFinished = newIndex == newEndIndexExcl;
                if (oldFinished || newFinished)
                {
                    return oldFinished == newFinished; // Same only if we have same number of parameters
                }

                // Since neither subtree has finished, it's safe to read the next frame from both
                ref var oldFrame = ref oldParameters.GetFrames()[oldIndex];
                ref var newFrame = ref @this.GetFrames()[newIndex];

                // Stop if we've reached the end of either subtree's sequence of attributes
                oldFinished = oldFrame.FrameType != RenderTreeFrameType.Attribute;
                newFinished = newFrame.FrameType != RenderTreeFrameType.Attribute;
                if (oldFinished || newFinished)
                {
                    return oldFinished == newFinished; // Same only if we have same number of parameters
                }

                var newValue = newFrame.AttributeValue;
                hasMutableParameters |= ChangeDetection.CannotDetectChange(newValue?.GetType());

                if (!string.Equals(oldFrame.AttributeName, newFrame.AttributeName, StringComparison.Ordinal))
                {
                    return false; // Different names
                }

                var oldValue = oldFrame.AttributeValue;

                if (ChangeDetection.IsChanged(oldValue, newValue))
                {
                    return false;
                }
            }
        }

        bool FurtherCheckMutableParameters()
        {
            while (true)
            {
                var newFinished = newIndex == newEndIndexExcl;
                if (newFinished || hasMutableParameters)
                {
                    return hasMutableParameters;
                }

                ref var newFrame = ref @this.GetFrames()[newIndex];
                hasMutableParameters |= ChangeDetection.CannotDetectChange(newFrame.AttributeValue?.GetType());
                newIndex++;
            }
        }

        return (!equals, hasMutableParameters);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetOwnerIndex(this in ParameterView @this)
    {
        return _ownerIndex(@this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static RenderTreeFrame[] GetFrames(this in ParameterView @this)
    {
        return _frames(@this)!;
    }
}