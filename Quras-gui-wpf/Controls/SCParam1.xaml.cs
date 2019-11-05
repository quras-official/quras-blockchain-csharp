using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Pure;
using Pure.SmartContract;
using Pure.Cryptography.ECC;

using Quras_gui_wpf.Utils;
using Quras_gui_wpf.Global;

namespace Quras_gui_wpf.Controls
{
    /// <summary>
    /// Interaction logic for SCParam1.xaml
    /// </summary>
    public partial class SCParam1 : UserControl
    {
        private LANG iLang => Constant.GetLang();

        ContractParameter parameter;
        public SCParam1()
        {
            InitializeComponent();
            InitInterface();
        }

        public SCParam1(ContractParameter param)
        {
            InitializeComponent();
            parameter = param;
            InitInterface();
        }
        
        private void InitInterface()
        {
            if (parameter == null)
            {
                return;
            }

            stackValues.Children.Clear();

            switch (parameter.Type)
            {
                case ContractParameterType.Signature:
                    {
                        cmbParamTypes.SelectedIndex = 0;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((byte[])parameter.Value).ToHexString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.Boolean:
                    {
                        cmbParamTypes.SelectedIndex = 1;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((bool)parameter.Value).ToString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.Integer:
                    {
                        cmbParamTypes.SelectedIndex = 2;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((BigInteger)parameter.Value).ToString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.Hash160:
                    {
                        cmbParamTypes.SelectedIndex = 3;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((UInt160)parameter.Value).ToString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.Hash256:
                    {
                        cmbParamTypes.SelectedIndex = 4;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((UInt256)parameter.Value).ToString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.ByteArray:
                    {
                        cmbParamTypes.SelectedIndex = 5;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((Byte[])parameter.Value).ToHexString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.PublicKey:
                    {
                        cmbParamTypes.SelectedIndex = 6;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = ((ECPoint)parameter.Value).ToString();
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.String:
                    {
                        cmbParamTypes.SelectedIndex = 7;

                        TextBox txbValue = new TextBox();
                        txbValue.BorderThickness = new Thickness(1);
                        txbValue.Width = 300;
                        txbValue.Height = 45;
                        txbValue.Margin = new Thickness(20, 2, 10, 0);
                        txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                        txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                        txbValue.VerticalAlignment = VerticalAlignment.Center;
                        txbValue.Text = (string)parameter.Value;
                        Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                        txbValue.Style = style;

                        stackValues.Children.Add(txbValue);
                        break;
                    }
                case ContractParameterType.Array:
                    {
                        cmbParamTypes.SelectedIndex = 8;

                        if (((List<ContractParameter>)parameter.Value).Count == 0)
                        {
                            StackPanel panArray = new StackPanel();
                            panArray.Orientation = Orientation.Horizontal;

                            TextBox txbValue = new TextBox();
                            txbValue.BorderThickness = new Thickness(1);
                            txbValue.Width = 250;
                            txbValue.Height = 45;
                            txbValue.Margin = new Thickness(20, 2, 10, 0);
                            txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                            txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                            txbValue.VerticalAlignment = VerticalAlignment.Center;
                            Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                            txbValue.Style = style;

                            Button btnAdd = new Button();
                            btnAdd.Content = "+";
                            Style btnStyle = Application.Current.FindResource("QurasSCParmAddButtonStyle") as Style;
                            btnAdd.Style = btnStyle;
                            btnAdd.Cursor = Cursors.Hand;
                            btnAdd.Opacity = 1;
                            btnAdd.Width = 35;
                            btnAdd.Height = 35;
                            btnAdd.FontWeight = FontWeights.Bold;
                            btnAdd.Click += BtnAdd_Click;

                            panArray.Children.Add(txbValue);
                            panArray.Children.Add(btnAdd);


                            stackValues.Children.Add(panArray);
                        }
                        else
                        {
                            int index = 0;
                            foreach(ContractParameter param in (List<ContractParameter>)parameter.Value)
                            {
                                StackPanel panArray = new StackPanel();
                                panArray.Orientation = Orientation.Horizontal;

                                TextBox txbValue = new TextBox();
                                txbValue.BorderThickness = new Thickness(1);
                                txbValue.Width = 250;
                                txbValue.Height = 45;
                                txbValue.Margin = new Thickness(20, 2, 10, 0);
                                txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
                                txbValue.HorizontalAlignment = HorizontalAlignment.Left;
                                txbValue.VerticalAlignment = VerticalAlignment.Center;
                                txbValue.Text = param.ToString();
                                Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
                                txbValue.Style = style;

                                panArray.Children.Add(txbValue);

                                if (index == 0)
                                {
                                    Button btnAdd = new Button();
                                    btnAdd.Content = "+";
                                    Style btnStyle = Application.Current.FindResource("QurasSCParmAddButtonStyle") as Style;
                                    btnAdd.Style = btnStyle;
                                    btnAdd.Cursor = Cursors.Hand;
                                    btnAdd.Opacity = 1;
                                    btnAdd.Width = 35;
                                    btnAdd.Height = 35;
                                    btnAdd.FontWeight = FontWeights.Bold;
                                    btnAdd.Click += BtnAdd_Click;
                                    panArray.Children.Add(btnAdd);
                                }
                                else
                                {
                                    Button btnDelete = new Button();
                                    btnDelete.Content = "-";
                                    Style btnStyle = Application.Current.FindResource("QurasSCParmAddButtonStyle") as Style;
                                    btnDelete.Style = btnStyle;
                                    btnDelete.Cursor = Cursors.Hand;
                                    btnDelete.Opacity = 1;
                                    btnDelete.Width = 35;
                                    btnDelete.Height = 35;
                                    btnDelete.FontWeight = FontWeights.Bold;
                                    btnDelete.Click += BtnDelete_Click;
                                    panArray.Children.Add(btnDelete);
                                }
                                stackValues.Children.Add(panArray);
                                index++;
                            }
                        }
                        break;
                    }
                default:
                    throw new ArgumentException();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panArray = new StackPanel();
            panArray.Orientation = Orientation.Horizontal;

            TextBox txbValue = new TextBox();
            txbValue.BorderThickness = new Thickness(1);
            txbValue.Width = 250;
            txbValue.Height = 45;
            txbValue.Margin = new Thickness(20, 2, 10, 0);
            txbValue.Tag = StringTable.GetInstance().GetString("STR_SCP_VALUE", iLang);
            txbValue.HorizontalAlignment = HorizontalAlignment.Left;
            txbValue.VerticalAlignment = VerticalAlignment.Center;
            Style style = Application.Current.FindResource("QurasSCParamsTextboxStyle") as Style;
            txbValue.Style = style;

            Button btnDelete = new Button();
            btnDelete.Content = "-";
            Style btnStyle = Application.Current.FindResource("QurasSCParmAddButtonStyle") as Style;
            btnDelete.Style = btnStyle;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Opacity = 1;
            btnDelete.Width = 35;
            btnDelete.Height = 35;
            btnDelete.FontWeight = FontWeights.Bold;
            btnDelete.Click += BtnDelete_Click;

            panArray.Children.Add(txbValue);
            panArray.Children.Add(btnDelete);
            
            stackValues.Children.Add(panArray);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            stackValues.Children.Remove((UIElement)((Button)sender).Parent);
        }

        public ContractParameter GetParameter()
        {
            UIElementCollection valueItems = stackValues.Children;
            switch ((string)((ComboBoxItem)cmbParamTypes.SelectedItem).Content)
            {
                case "Signature":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        
                        break;
                    }
                case "Boolean":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "Integer":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "Hash160":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "Hash256":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "ByteArray":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "PublicKey":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "String":
                    {
                        string value = ((TextBox)valueItems[0]).Text;

                        try
                        {
                            parameter.SetValue(value);
                        }
                        catch (FormatException fex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_FAIL_TYPE_SIGNATURE", iLang));
                        }
                        catch (ArgumentException aex)
                        {
                            StaticUtils.ShowMessageBox(StaticUtils.ErrorBrush, StringTable.GetInstance().GetString("STR_ERR_UNKOWN_TYPE_SIGNATURE", iLang));
                        }
                        break;
                    }
                case "Array":
                    {
                        ((List<ContractParameter>)parameter.Value).Clear();
                        foreach (StackPanel panItem in valueItems)
                        {
                            string value = ((TextBox)panItem.Children[0]).Text;

                            ContractParameter param = new ContractParameter();

                            if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
                            {
                                param.Type = ContractParameterType.Boolean;
                                param.Value = true;
                            }
                            else if (string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
                            {
                                param.Type = ContractParameterType.Boolean;
                                param.Value = false;
                            }
                            else if (long.TryParse(value, out long num))
                            {
                                param.Type = ContractParameterType.Integer;
                                param.Value = num;
                            }
                            else if (value.StartsWith("0x"))
                            {
                                if (UInt160.TryParse(value, out UInt160 i160))
                                {
                                    param.Type = ContractParameterType.Hash160;
                                    param.Value = i160;
                                }
                                else if (UInt256.TryParse(value, out UInt256 i256))
                                {
                                    param.Type = ContractParameterType.Hash256;
                                    param.Value = i256;
                                }
                                else if (BigInteger.TryParse(value.Substring(2), NumberStyles.AllowHexSpecifier, null, out BigInteger bi))
                                {
                                    param.Type = ContractParameterType.Integer;
                                    param.Value = bi;
                                }
                                else
                                {
                                    param.Type = ContractParameterType.String;
                                    param.Value = value;
                                }
                            }
                            else if (ECPoint.TryParse(value, ECCurve.Secp256r1, out ECPoint point))
                            {
                                param.Type = ContractParameterType.PublicKey;
                                param.Value = point;
                            }
                            else
                            {
                                try
                                {
                                    param.Value = value.HexToBytes();
                                    param.Type = ContractParameterType.ByteArray;
                                }
                                catch (FormatException)
                                {
                                    param.Type = ContractParameterType.String;
                                    param.Value = value;
                                }
                            }

                            ((List<ContractParameter>)parameter.Value).Add(param);
                        }
                        break;
                    }
            }

            return parameter;
        }

        private void cmbSmartContractParamType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch ((string)((ComboBoxItem)cmbParamTypes.SelectedItem).Content)
            {
                case "Signature":
                    {
                        break;
                    }
                case "Boolean":
                    {
                        break;
                    }
                case "Integer":
                    {
                        break;
                    }
                case "Hash160":
                    {
                        break;
                    }
                case "Hash256":
                    {
                        break;
                    }
                case "ByteArray":
                    {
                        break;
                    }
                case "PublicKey":
                    {
                        break;
                    }
                case "String":
                    {
                        break;
                    }
                case "Array":
                    {
                        break;
                    }
            }
        }
    }
}
