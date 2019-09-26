using Android.App;
using Android.Widget;
using Android.OS;
using RaysHotDogs.Core;
using Android.Graphics;
using System.Net;
using RaysHotDogs.Utility;
using System;
using Android.Content;

namespace RaysHotDogs
{
    [Activity(Label = "Details", Icon = "@drawable/delivery1")]
    public class HotDogDetailActivity : Activity
    {
        private ImageView hotDogImageView;
        private TextView hotDogNameTextView;
        private TextView shortDescriptionTextView;
        private TextView descriptionTextView;
        private TextView priceTextView;
        private EditText amountEditText;
        private Button cancelButton;
        private Button orderButton;
        private HotDog selectedHotDog;
        private HotDogDataService dataService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HotDogDetailView);

            var selectedHotDogId = Intent.Extras.GetInt("selectedHotDogId");

            dataService = new HotDogDataService();
            selectedHotDog = dataService.GetHotDogById(selectedHotDogId);

            FindViews();

            BindData();

            HandleEvents();
        }

        private void HandleEvents()
        {
            orderButton.Click += (object sender, EventArgs e) =>
            {
                var amount = Int32.Parse(amountEditText.Text);
                AddToCart(selectedHotDog, amount);


                var intent = new Intent();
                intent.PutExtra("selectedHotDogId", selectedHotDog.HotDogId);
                intent.PutExtra("amount", amount);

                SetResult(Result.Ok, intent);

                this.Finish();
            };

            cancelButton.Click += (object sender, System.EventArgs e) =>
            {
                SetResult(Result.Canceled);

                this.Finish();
            };

        }

        public void AddToCart(HotDog hotDog, int amount)
        {
            CartDataService cartDataService = new CartDataService();
            cartDataService.AddCartItem(hotDog, amount);
        }

        private void FindViews()
        {
            hotDogImageView = FindViewById<ImageView>(Resource.Id.hotDogImageView);
            hotDogNameTextView = FindViewById<TextView>(Resource.Id.hotDogNameTextView);
            shortDescriptionTextView = FindViewById<TextView>(Resource.Id.shortDescriptionTextView);
            descriptionTextView = FindViewById<TextView>(Resource.Id.descriptionTextView);
            priceTextView = FindViewById<TextView>(Resource.Id.priceTextView);
            amountEditText = FindViewById<EditText>(Resource.Id.amountEditText);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            orderButton = FindViewById<Button>(Resource.Id.orderButton);
        }

        private void BindData()
        {

            hotDogNameTextView.Text = selectedHotDog.Name;
            shortDescriptionTextView.Text = selectedHotDog.ShortDescription;
            descriptionTextView.Text = selectedHotDog.Description;
            priceTextView.Text = "Price: " + selectedHotDog.Price;

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl("http://gillcleerenpluralsight.blob.core.windows.net/files/" + selectedHotDog.ImagePath + ".jpg");

            hotDogImageView.SetImageBitmap(imageBitmap);
        }
    }
}


