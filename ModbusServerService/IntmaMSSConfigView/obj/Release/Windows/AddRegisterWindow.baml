   M S B A M L   `   `   `  ���� M  IIntmaMSSConfigView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Intma.ModbusServerService.Configurator.Windows.Intma.ModbusServerService.Configurator.Windows  F    @Intma.ModbusServerService.Configurator.Windows.AddRegisterWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Nlocal<clr-namespace:Intma.ModbusServerService.Configurator.Windows       � WindowStartupLocation$  CenterScreen=� � Title$+ %Добавление регистра��$	��340��$	��400��.��� .+��  � RowDefinitions �� $
��auto ��� �� �� .+��� $��0��$:��4Путь в XML (Через запутяную ','):��$��10 0 0 0q�$
��auto��$
��auto��$	��Top=�$
��Left=��� -   $��tbPath��$��0��$��25��$
��auto��$��10 30 10 10q�$��Bottom=����� *Path+�� $��1��$��Тип Данных:��$��10 0 0 0q�$
��auto��$
��auto��$	��Top=�$
��Left=��� -   $��cmbType��$��1�� �� 
IsEditable . $��10 30 10 10q�$��25��$
��auto��{�.  �� 
IsReadOnly . ���� *MbtcpDataTypes+ �� SelectedItem �� *
DataType+�� $��2��$��Регистр:��$��10 0 0 0q�$
��auto��$
��auto��$	��Top=�$
��Left=��� -   $��tbReg1��$��2��$��25��$
��auto��$��10 30 10 10q�$��Bottom=����� *ValueRegister+�� $��3��$��	0 -28 0 0q�
��������� $��3��$��	5,-20,5,0q�
����İ��� -   $��3��$��Добавить��$
��auto��$
��auto��$��Bottom=�$
��Left=�$��
30 30 0 15q�$��6 3 6 3q�$��14(��� -   $��3��$��Отмена��$
��auto��$
��auto��$��Bottom=�$
��Left=�$��118 30 0 15q�$��6 3 6 3q�$��14(�