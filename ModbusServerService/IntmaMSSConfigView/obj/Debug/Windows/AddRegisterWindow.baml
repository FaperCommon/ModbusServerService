   M S B A M L   `   `   `  ����M  IIntmaMSSConfigView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Intma.ModbusServerService.Configurator.Windows.Intma.ModbusServerService.Configurator.Windows  5   	   F    @Intma.ModbusServerService.Configurator.Windows.AddRegisterWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Nlocal<clr-namespace:Intma.ModbusServerService.Configurator.Windows     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$+ %Добавление регистра��5	   	   $	��340��6%   $	��400��62   .��5
      � .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      �� 5      �� 5      .+�5      �� $��0��6   $:��4Путь в XML (Через запутяную ','):��6!   $��10 0 0 0q�6M   $
��auto��6_   $
��auto��6m   $	��Top=�6z   $
��Left=�6�   �� 5      -   5        $��tbPath��5   :   $��0��6   $��25��6H   $
��auto��6T   $��10 30 10 10q�6a   $��Bottom=�6v   ��6$   �� *Path+6v   �� 5      $��1��6   $��Тип Данных:��6!   $��10 0 0 0q�67   $
��auto��6I   $
��auto��6W   $	��Top=�6d   $
��Left=�6|   �� 5      -   5        $��cmbType��5      $��1��6`    �� 
IsEditable . $��10 30 10 10q�6�   $��25��6�   $
��auto��6�   {�.  �� 
IsReadOnly . ��6   �� *MbtcpDataTypes+ �� SelectedItem 6>   �� *
DataType+6�   �� 5      $��2��6   $��Регистр:��6!   $��10 0 0 0q�64   $
��auto��6F   $
��auto��6T   $	��Top=�6a   $
��Left=�6y   �� 5      -   5        $��tbReg1��5   C   $��2��65   $��25��6Q   $
��auto��6]   $��10 30 10 10q�6j   $��Bottom=�6   ��6   �� *ValueRegister+6�   �� 5      $��3��6   $��	0 -28 0 0q�6%   
�������6R   �� 5      $��3��6   $��	5,-20,5,0q�6%   
����İ�6X   �� 5       -   5        $��3��5       $��Добавить��6"   $
��auto��66   $
��auto��6D   $��Bottom=�6Q   $
��Left=�5!      $��
30 30 0 15q�6,   $��6 3 6 3q�6@   $��14(�5"   )   �� 5#      -   5        $��3��5#      $��Отмена��6"   $
��auto��64   $
��auto��6B   $��Bottom=�6O   $
��Left=�5$      $��118 30 0 15q�6,   $��6 3 6 3q�6A   $��14(�6S   5%      5&      5'      5(      