﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Splat;
using Xamarin.Forms;


namespace Smartdocs
{
    public class StandardViewModel : AbstractViewModel
    {
        public StandardViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Alert = this.Create(async token => await this.Dialogs.AlertAsync("", "Workitem Action submitted Succesfully", null, token));
			this.AlertFail= this.Create(async token => await this.Dialogs.AlertAsync("", "Failed to submit workitem", null, token));

            this.AlertLongText = this.Create(async token =>
                await this.Dialogs.AlertAsync("",
                cancelToken: token
                )
            );

            this.ActionSheet = this.CreateActionSheetCommand(false);
            this.BottomSheet = this.CreateActionSheetCommand(true);

            this.ActionSheetAsync = this.Create(async token =>
            {
                var result = await this.Dialogs.ActionSheetAsync("Test Title", "Cancel", "Destroy", token, "Button1", "Button2", "Button3");
                this.Result(result);
            });

            this.Confirm = this.Create(async token =>
            {
                var r = await this.Dialogs.ConfirmAsync("Pick a choice", "Pick Title", cancelToken: token);
                var text = r ? "Yes" : "No";
                this.Result($"Confirmation Choice: {text}");
            });

            this.Login = this.Create(async token =>
            {
                var r = await this.Dialogs.LoginAsync(new LoginConfig
                {
                    Message = "DANGER"
                }, token);
                var status = r.Ok ? "Success" : "Cancelled";
                this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
            });

            this.Prompt = new Command(() => this.Dialogs.ActionSheet(new ActionSheetConfig()
                .SetTitle("Choose Type")
                .Add("Default", () => this.PromptCommand(InputType.Default))
                .Add("E-Mail", () => this.PromptCommand(InputType.Email))
                .Add("Name", () => this.PromptCommand(InputType.Name))
                .Add("Number", () => this.PromptCommand(InputType.Number))
                .Add("Number with Decimal", () => this.PromptCommand(InputType.DecimalNumber))
                .Add("Password", () => this.PromptCommand(InputType.Password))
                .Add("Numeric Password (PIN)", () => this.PromptCommand(InputType.NumericPassword))
                .Add("Phone", () => this.PromptCommand(InputType.Phone))
                .Add("Url", () => this.PromptCommand(InputType.Url))
                .SetCancel()
            ));
            this.PromptApproveComment = this.Create(async token =>
            {
                var result = await this.Dialogs.PromptAsync(new PromptConfig
                {
                    Title = "Please enter comment",
                   Placeholder = "Please enter comment",
                Text = "",
					IsCancellable = true
                }, token);
				App.approveComment = result.Text;
                //this.Result($"Result - {result.Text}");
            });

            this.Date = this.Create(async token =>
            {
                var result = await this.Dialogs.DatePromptAsync(new DatePromptConfig
                {
                    IsCancellable = true,
                    MinimumDate = DateTime.Now.AddDays(-3),
                    MaximumDate = DateTime.Now.AddDays(1)
                }, token);
                this.Result($"Date Prompt: {result.Ok} - Value: {result.SelectedDate}");
            });
            this.Time = this.Create(async token =>
            {
                var result = await this.Dialogs.TimePromptAsync(new TimePromptConfig
                {
                    IsCancellable = true
                }, token);
                this.Result($"Time Prompt: {result.Ok} - Value: {result.SelectedTime}");
            });
            this.Time24 = this.Create (async token => {
                var result = await this.Dialogs.TimePromptAsync(new TimePromptConfig {
                    IsCancellable = true,
                    Use24HourClock = true
                }, token);
                this.Result ($"Time Prompt: {result.Ok} - Value: {result.SelectedTime}");
            });
        }


        CancellationTokenSource cancelSrc;

        bool autoCancel;
        public bool AutoCancel
        {
            get { return this.autoCancel; }
            set
            {
                if (this.autoCancel == value)
                    return;

                this.autoCancel = value;
                this.OnPropertyChanged();
                if (value)
                    this.cancelSrc = new CancellationTokenSource();
                else
                    this.cancelSrc = null;
            }
        }


        public ICommand Alert { get; }
		public ICommand AlertFail { get; }
        public ICommand AlertLongText { get; }
        public ICommand ActionSheet { get; }
        public ICommand ActionSheetAsync { get; }
        public ICommand BottomSheet { get; }
        public ICommand Confirm { get; }
        public ICommand Login { get; }
        public ICommand Prompt { get; }
        public ICommand PromptApproveComment { get; }
        public ICommand Date { get; }
        public ICommand Time { get; }
        public ICommand Time24 { get; }


        ICommand CreateActionSheetCommand(bool useBottomSheet)
        {
            return new Command(() =>
            {
                var cfg = new ActionSheetConfig()
                    .SetTitle("Test Title")
                    .SetUseBottomSheet(useBottomSheet);

                IBitmap testImage = null;
                try
                {
                    testImage = BitmapLoader.Current.LoadFromResource("icon.png", null, null).Result;
                }
                catch
                {
                    Debug.WriteLine("Could not load image");
                }

                for (var i = 0; i < 5; i++)
                {
                    var display = (i + 1);
                    cfg.Add(
                        "Option " + display,
                        () => this.Result($"Option {display} Selected"),
                        testImage
                    );
                }
                cfg.SetDestructive(null, () => this.Result("Destructive BOOM Selected"), testImage);
                cfg.SetCancel(null, () => this.Result("Cancel Selected"), testImage);

                var disp = this.Dialogs.ActionSheet(cfg);
                if (this.AutoCancel)
                {
                    Task.Delay(TimeSpan.FromSeconds(3))
                        .ContinueWith(x => disp.Dispose());
                }
            });
        }


        ICommand Create(Func<CancellationToken?, Task> action)
        {
            return new Command(async () =>
            {
                try
                {
                    this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
                    await action(this.cancelSrc?.Token);
                }
                catch (OperationCanceledException)
                {
                    if (this.AutoCancel)
                        this.cancelSrc = new CancellationTokenSource();

                    this.Dialogs.Alert("Task cancelled successfully");
                }
            });
        }


        async Task PromptCommand(InputType inputType)
        {
            var msg = $"Enter a {inputType.ToString().ToUpper()} value";
            this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
            var r = await this.Dialogs.PromptAsync(msg, inputType: inputType, cancelToken: this.cancelSrc?.Token);
            this.Result(r.Ok
                ? "OK " + r.Text
                : "Prompt Cancelled");
        }
    }
}
