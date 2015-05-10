using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Sheva.Internal;

namespace Sheva.Windows.Controls
{
    public enum TransitionEffects
    {
        None,
        Wiping,
        Closing,
        Opening,
        Fading,
        Sliding,
        Waving,
    }

[TemplatePartAttribute(Name = "PART_ContentHost", Type = typeof(ContentPresenter))]
[TemplatePartAttribute(Name = "PART_StaleContentHost", Type = typeof(ContentPresenter))]
public class TransitionControl : ContentControl
{
    private static readonly DependencyPropertyKey IsContentChangedPropertyKey;
    public static readonly DependencyProperty StaleContentProperty;
    public static readonly DependencyProperty IsContentChangedProperty;
    public static readonly DependencyProperty TransitionEffectProperty;
    private static ResourceKey defaultTemplateKey = null;
    private static ResourceKey wipingTemplateKey = null;
    private static ResourceKey closingTemplateKey = null;
    private static ResourceKey openingTemplateKey = null;
    private static ResourceKey fadingTemplateKey = null;
    private static ResourceKey slidingTemplateKey = null;
    private static ResourceKey wavingTemplateKey = null;

    static TransitionControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TransitionControl),
            new FrameworkPropertyMetadata(typeof(TransitionControl)));

        TransitionEffectProperty = DependencyProperty.Register(
           "TransitionEffect",
           typeof(TransitionEffects),
           typeof(TransitionControl),
           new FrameworkPropertyMetadata(TransitionEffects.None));

        StaleContentProperty = DependencyProperty.Register(
            "StaleContent",
            typeof(Object),
            typeof(TransitionControl),
            new FrameworkPropertyMetadata(null));

        ContentControl.ContentProperty.OverrideMetadata(
            typeof(TransitionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentChanged)));

        IsContentChangedPropertyKey = DependencyProperty.RegisterReadOnly(
           "IsContentChanged",
           typeof(Boolean),
           typeof(TransitionControl),
           new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

        IsContentChangedProperty = IsContentChangedPropertyKey.DependencyProperty;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Boolean IsContentChanged
    {
        get { return (Boolean) GetValue(IsContentChangedProperty); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Object StaleContent
    {
        get { return base.GetValue(StaleContentProperty); }
        set { base.SetValue(StaleContentProperty, value); }
    }

    public TransitionEffects TransitionEffect
    {
        get { return (TransitionEffects) base.GetValue(TransitionEffectProperty); }
        set { base.SetValue(TransitionEffectProperty, value); }
    }

    public static ResourceKey DefaultTemplateKey
    {
        get
        {
            if (defaultTemplateKey == null)
            {
                defaultTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "DefaultTemplateKey");
            }
            return defaultTemplateKey;
        }
    }

    public static ResourceKey WavingTemplateKey
    {
        get
        {
            if (wavingTemplateKey == null)
            {
                wavingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "WavingTemplateKey");
            }
            return wavingTemplateKey;
        }
    }

    public static ResourceKey SlidingTemplateKey
    {
        get
        {
            if (slidingTemplateKey == null)
            {
                slidingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "SlidingTemplateKey");
            }
            return slidingTemplateKey;
        }
    }

    public static ResourceKey WipingTemplateKey
    {
        get
        {
            if (wipingTemplateKey == null)
            {
                wipingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "WipingTemplateKey");
            }
            return wipingTemplateKey;
        }
    }

    public static ResourceKey FadingTemplateKey
    {
        get
        {
            if (fadingTemplateKey == null)
            {
                fadingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "FadingTemplateKey");
            }
            return fadingTemplateKey;
        }
    }

    public static ResourceKey OpeningTemplateKey
    {
        get
        {
            if (openingTemplateKey == null)
            {
                openingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "OpeningTemplateKey");
            }
            return openingTemplateKey;
        }
    }

    public static ResourceKey ClosingTemplateKey
    {
        get
        {
            if (closingTemplateKey == null)
            {
                closingTemplateKey = new ComponentResourceKey(typeof(TransitionControl), "ClosingTemplateKey");
            }
            return closingTemplateKey;
        }
    }

    private static void OnContentChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        TransitionControl tc = element as TransitionControl;
        tc.SetValue(IsContentChangedPropertyKey, BooleanBoxes.FalseBox);
        tc.StaleContent = e.OldValue;
        if (e.OldValue != null && e.NewValue != null)
        {
            if (!tc.IsLoaded)
            {
                tc.Loaded += delegate(Object sender, RoutedEventArgs args)
                {
                    tc.SetValue(IsContentChangedPropertyKey, BooleanBoxes.TrueBox);
                };
            }
            else
            {
                tc.SetValue(IsContentChangedPropertyKey, BooleanBoxes.TrueBox);
            }
        }
    }
}
}
