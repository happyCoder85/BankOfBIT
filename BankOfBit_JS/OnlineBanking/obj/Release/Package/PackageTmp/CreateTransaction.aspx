<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="CreateTransaction.aspx.cs" Inherits="OnlineBanking.CreateTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="AccountNumber" runat="server" Text="Account Number:"></asp:Label>
        <asp:Label ID="lblCreateTransactionAccountNumber" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Balance" runat="server" Text="Balance:"></asp:Label>
        <asp:Label ID="lblCreateTransactionBalance" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblTransactionType" runat="server" Text="Transaction Type:"></asp:Label>
        <asp:DropDownList ID="ddlTransactionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
        </asp:DropDownList>
    </p>
    <p>
        <asp:Label ID="Amount" runat="server" Text="Amount:"></asp:Label>
        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvRequiredFieldValidator" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rvRangeValidator" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="RangeValidator" MaximumValue="10000.00" MinimumValue="0.01" Enabled="False"></asp:RangeValidator>
        <br />
    </p>
    <p>
        <asp:Label ID="To" runat="server" Text="To:"></asp:Label>
        <asp:DropDownList ID="ddlTo" runat="server">
        </asp:DropDownList>
    </p>
    <p>
        <asp:LinkButton ID="lbCompleteTransaction" runat="server" Width="175px" OnClick="lbCompleteTransaction_Click">Complete Transaction</asp:LinkButton>
        <asp:LinkButton ID="lbReturnToAccounts" runat="server" CausesValidation="False" OnClick="lbReturnToAccounts_Click">Return to Account</asp:LinkButton>
    </p>
    <p>
        <asp:Label ID="lblCreateTransactionError" runat="server" Text="Label" Visible="False" ForeColor="Red"></asp:Label>
    </p>
</asp:Content>
