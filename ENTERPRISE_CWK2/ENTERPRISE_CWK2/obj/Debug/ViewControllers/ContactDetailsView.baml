   M S B A M L   `   `   `  ����J  FENTERPRISE_CWK2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ENTERPRISE_CWK2.ViewControllersENTERPRISE_CWK2.ViewControllers  5      8    2ENTERPRISE_CWK2.ViewControllers.ContactDetailsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:ENTERPRISE_CWK2.ViewControllers     5         userControlStyle#��[  5      .��5	      �   � ColumnDefinitions  5   
   �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�51   
   (� $��0��6   $��0��6"   $
��Left=�6/   .��52      ��   largeLabelStyle#��[ 6   .��6>   Contact Details6O   53      (� 55   
   $��1��6   $��2��6"   $��0��66   $��Right=�6C   .��56      �� -   5         ��Name$ contactButtonsStackPanel��56       �� Orientation$ 
Horizontal=�6;   58      59      �� 5;   
   $��0��6   $��1��6!   $��3��6.     YellowColor#��[ 6B   $��
10,0,10,10q�6l   ._�5<      (� .��5=      �� -   5         ��Name$ contactNameLabel��5=      $��Stretch=�62   .��6Y   
John Jones6e   5>      5?      � 5A   
   $��2��6   $��0��6   $��3��6,   $��2��6@   $��	10,0,10,0q�6Q     5B      �� 5C      �� 5D      �� 5E      .+�5J      �� $��0��6     
WhiteColor#��[ 6%     VioletColor#��[ 6N   $��2q�5K      ._�5L      (� .��5M      �� $ Vertical=�6!   .+�5N      �� -   5        $ contactAccDetailsLabel��5N       $&�� Acc.:09129323 SortCode: 60-20-31��6@     
GreenColor#��[ 6k   #��[ 5O       $��16(�6J   $��5,0,5,0q�6X   �� 5Q      -   5        $ contactPhoneLabel��5Q       $��Phone:07851324345��6;   $��Center=�6W   $��22(�6{   $��5,0,5,0q�6�   �� 5R      -   5        $ contactEmailLabel��5R       $��Email: j.jones@mail.com��6;   $��Center=�6]   $��22(�6�   $��5,0,5,0q�6�   5T      5V      5X      �� 5[      $��1��6   #��[ 6%   #��[ 6N   $��2q�5\      ._�5]      (� .��5^      �� .+�5_      �� $��Center=�6    .��6D   Total Paid:6Q   �� 5`      -   5        $ contactTotalPaidLabel��5`       $��£13,430,410��6?   $��Center=�6U   5a      5c      5e      �� 5g      $��2��6   #��[ 6%   #��[ 6N   $��2q�5h      ._�5i      (� .��5j      �� .+�5k      �� $��Center=�6    .��6D   Total Received:6U   �� 5l      -   5        $ contactTotalReceived��5l       $��£11,210,395��6>   $��Center=�6T   5m      5o      5q      5s      (� 5v   
   $��0��6   $��4��6"   $��3��6/   $
��Left=�6C   .��5w      �� -   5        $ contactTransactionsLabel��5w      #��[ 66   .��6`   Transactions6n   5x      &   System.Windows.Controls.DataGrid  5z   
   -	   5          Name$ contactsDataGrid��5{      $��5��5z      $��0��6    $��5��60   $��3��6A   $��10q�6U     dataGridStyle#��[ 5}      #��[ 6B   $��0q�6l      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$
 Type��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6\   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 63   �� 
 �� Path$
 TransactionType�6\     5�      $
 Name��6%   $ 2* 6\   	 63   �� $
 TransactionName�6\     5�      $
 Date��6%   $ * 6l   	 63   �� $
 TransactionDate� �� StringFormat$ d��6l     5�      $ Account��6%   $ * 6a   	 66   �� $
 TransactionAccNum�6a     5�      $ 	Sort Code��6%   $ * 6e   	 68   �� $
 TransactionSortCode�6e     5�      $ 	Reference��6%   $ 2* 6f   	 68   �� $
 TransactionReference�6f     5�      $ Amount��6%   $ * 6f   	 65   �� $
 TransactionAmountString�6f   5�      5�      5�      