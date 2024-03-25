<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransactionListing.aspx.cs" Inherits="OnlineBanking.TransactionListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="lblTransactionClientName" runat="server" Text="Label" Font-Bold="True" Font-Size="Large"></asp:Label>
        <br />
    </p>
    <p>
        <asp:Label ID="AccountNumber" runat="server" Text="Account Number:" Width="125px"></asp:Label>
        <asp:Label ID="lblTransactionAccountNumber" runat="server" Text="Label" Width="75px"></asp:Label>
        <asp:Label ID="AccountBalance" runat="server" Text="Balance:" Width="60px"></asp:Label>
        <asp:Label ID="lblTransactionAccountBalance" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
            <Columns>
                <asp:BoundField DataField="DateCreated" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Date">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="TransactionType.Description" HeaderText="Transaction Type">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="250px" />
                </asp:BoundField>
                <asp:BoundField DataField="Deposit" DataFormatString="{0:c}" HeaderText="Amount In">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="Withdrawal" DataFormatString="{0:c}" HeaderText="Amount Out">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Details">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="300px" />
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
    </p>
    <p>
        <asp:LinkButton ID="lbTransactionWebForm" runat="server" OnClick="lbTransactionWebForm_Click" Width="250px">Pay Bills and Transfer Funds</asp:LinkButton>
        <asp:LinkButton ID="lbAccountListingWebForm" runat="server" OnClick="lbAccountListingWebForm_Click">Return to Account Listing</asp:LinkButton>
    </p>
    <p>
        <asp:Label ID="lblTransactionError" runat="server" Text="Label" ForeColor="Red" Visible="False"></asp:Label>
    </p>
    <p>
    </p>
</asp:Content>
