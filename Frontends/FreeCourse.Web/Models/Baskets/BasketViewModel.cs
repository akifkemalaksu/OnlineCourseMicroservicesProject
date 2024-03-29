﻿using System.Drawing;

namespace FreeCourse.Web.Models.Baskets
{
    public class BasketViewModel
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemViewModel> _basketItems;
        public decimal TotalPrice { get => _basketItems.Sum(x => x.GetCurrentPrice); }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }

        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                    });

                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }

        public void ApplyDiscount(string discountCode, int discountRate)
        {
            this.DiscountCode = discountCode;
            this.DiscountRate = discountRate;
        }
        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }
    }
}
