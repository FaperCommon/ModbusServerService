   M S B A M L   `   `   `  ����M  IIntmaMSSConfigView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Intma.ModbusServerService.Configurator.Windows.Intma.ModbusServerService.Configurator.Windows  5   	   G    AIntma.ModbusServerService.Configurator.Windows.AddWebSourceWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Nlocal<clr-namespace:Intma.ModbusServerService.Configurator.Windows     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Источник��5	      $	��350��6   $	��400��6&   .��5
      � .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      �� 5      �� 5      .+�5      �� $��0��6   $��Адрес:��6!   $��10 0 0 0q�62   $
��auto��6D   $
��auto��6R   $	��Top=�6_   $
��Left=�6w   �� 5      -   5        $��	tbAddress��5   @   $��0��62   $��25��6Q   $
��auto��6]   $��10 30 10 10q�6j   $��Bottom=�6   ��6   �� *
WebAddress+6   �� 5      $��1��6   $(��"Частота опроса, (с):��6!   $��10 0 0 0q�6@   $
��auto��6R   $
��auto��6`   $	��Top=�6m   $
��Left=�6�   �� 5      -   5        $��tbDur��5   >   $��1��60   $��25��6K   $
��auto��6W   $��10 30 10 10q�6d   $��Bottom=�6y   ��6   �� *
Duration+6y   �� 5      $��2��6   $$��Имя подключения:��6!   $��10 0 0 0q�6<   $
��auto��6N   $
��auto��6\   $	��Top=�6i   $
��Left=�6�   �� 5      -   5        $��tbName��5   :   $��2��6,   $��25��6H   $
��auto��6T   $��10 30 10 10q�6a   $��Bottom=�6v   ��6   �� *Name+6v   �� 5      $��3��6   $��	0 -28 0 0q�6%   
�������6R   �� 5      $��3��6   $��	5,-20,5,0q�6%   
����İ�6X   �� 5      -   5        $��	btnAccept��5   "   $��3��6   $��Добавить��63   $
��auto��6G   $
��auto��6U   $��Bottom=�6b   $
��Left=�5       $��
30 30 0 15q�6,   $��6 3 6 3q�6@   $��14(�5!   )   �� 5"      -   5        $��3��5"      $��Отмена��6"   $
��auto��64   $
��auto��6B   $��Bottom=�6O   $
��Left=�5#      $��118 30 0 15q�6,   $��6 3 6 3q�6A   $��14(�6S   5$      5%      5&      5'      