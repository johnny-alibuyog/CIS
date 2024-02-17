﻿using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Common.Camera;

public class CameraDialogController : ControllerBase<CameraDialogViewModel>
{
    public CameraDialogController(CameraDialogViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Accept = new ReactiveCommand();
        this.ViewModel.Accept.Subscribe(x => Accept());
        this.ViewModel.Accept.ThrownExceptions.Handle(this);

        //this.ViewModel.Camera.Start.Execute(null);
    }

    public virtual void Accept()
    {
        this.ViewModel.Close();
        this.ViewModel.Camera.Close();
    }
}