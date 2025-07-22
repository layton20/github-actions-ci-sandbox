using BookWrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWrom.Pages.Orders;

public class PlaceOrderModel : PageModel
{
    private readonly MessageSenderService __MessageSender;

    public PlaceOrderModel(MessageSenderService messageSender)
    {
        __MessageSender = messageSender;
    }

    public string SuccessMessage { get; private set; }

    [BindProperty]
    public string OrderId { get; set; }

    public async Task OnGetAsync()
    {
        string _Order = "Order details: Book ID 123, Quantity 2";

        // Send the order details as a message to the Service Bus queue
        await __MessageSender.SendMessageAsync(_Order);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(OrderId))
        {
            return Page();
        }

        await __MessageSender.SendMessageAsync($"Order submitted: {OrderId}");

        SuccessMessage = $"Order {OrderId} sent!";

        ModelState.Clear();

        return Page();
    }
}