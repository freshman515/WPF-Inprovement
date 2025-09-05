using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegeionNavigationDemo1.Interfaces;

public interface IDialogResultProvider<out TResult> {
    TResult? GetDialogResult();
}