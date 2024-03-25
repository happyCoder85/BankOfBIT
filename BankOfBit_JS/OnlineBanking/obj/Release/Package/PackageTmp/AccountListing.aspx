<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountListing.aspx.cs" Inherits="OnlineBanking.AccountListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="lblClientName" runat="server" Text="Label" Font-Bold="True" Font-Size="Large"></asp:Label>
    </p>
        <asp:GridView ID="gvClientAccounts" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" OnSelectedIndexChanged="gvClientAccounts_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="100px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Account Notes">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="300px" />
                </asp:BoundField>
                <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:c}">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Right" Width="200px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="Gray" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    <p>
        <asp:Label ID="lblError" runat="server" Text="Label" Visible="False" ForeColor="Red"></asp:Label>
    </p>
    <p>
    </p>
</asp:Content>
