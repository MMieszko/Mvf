using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;
using Mvf.Core.Extensions;

namespace Mvf.Core.Commands
{
    public class MvfCommandDispatcher<TViewModel>
        where TViewModel : IMvfViewModel
    {
        public TViewModel ViewModel { get; }

        public MvfCommandDispatcher(TViewModel viewModel, IEnumerable<Control> commandableControls)
        {
            this.ViewModel = viewModel;
            this.Initialize(commandableControls);
        }

        private void Initialize(IEnumerable<Control> controls)
        {
            var properties = ViewModel.GetType()
                .GetProperties()
                .Where(x => x.HasAttribute<MvfCommandable>())
                .ToList();

            foreach (var commandProperty in properties)
            {
                var controlName = commandProperty.GetPropertyFromAttribute<MvfCommandable, string>(x => x.ControlName);
                var eventName = commandProperty.GetPropertyFromAttribute<MvfCommandable, string>(x => x.EventName);
                var mvfCommand = commandProperty.GetValue(ViewModel, null) as MvfCommand;


                var control = controls.FirstOrDefault(x => x.Name == controlName);

                if (control == null || mvfCommand == null) continue;

                var @event = control.GetType().GetEvent(eventName);

                @event.AddEventHandler(control,
                    Delegate.CreateDelegate(@event.EventHandlerType, mvfCommand, mvfCommand.GetType().GetMethod(nameof(MvfCommand.ExecuteImplementation)) ?? throw new InvalidOperationException("Could not find execution implementations")));

            }
        }
    }
}
