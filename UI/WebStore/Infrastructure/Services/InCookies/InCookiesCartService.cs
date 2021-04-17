using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Mapping;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProductData _ProductData;
        private readonly string _CartName;

        private Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;
                var cart_cookies = context.Request.Cookies[_CartName];

                if (cart_cookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookies);
                return JsonConvert.DeserializeObject<Cart>(cart_cookies);
            }
            set => ReplaceCookies(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }


        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie);
        }

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ProductData = ProductData;
            var user = HttpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{user_name}";
        }

        public void Add(int id)
        {
            var _cart = Cart;

            var item = _cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
                _cart.Items.Add(new CartItem { ProductId = id });
            else
                item.Quantity++;

            Cart = _cart;
        }

        public void Decrement(int id)
        {
            var _cart = Cart;

            var item = _cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                _cart.Items.Remove(item);

            Cart = _cart;
        }

        public void Remove(int id)
        {
            var _cart = Cart;

            var item = _cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            _cart.Items.Remove(item);

            Cart = _cart;
        }

        public void Clear()
        {
            var _cart = Cart;
            _cart.Items.Clear();
            Cart = _cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                    .Where(item => product_view_models.ContainsKey(item.ProductId))
                    .Select(item => (product_view_models[item.ProductId], item.Quantity))
            };
        }
    }

}
