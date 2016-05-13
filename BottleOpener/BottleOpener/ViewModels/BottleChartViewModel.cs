using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using BottleOpener.DataAccess;
using BottleOpener.Models;

using OxyPlot;

namespace BottleOpener.ViewModels
{
    public class BottleChartViewModel
    {
        private const int UpdateInterval = 20;
        BottleLogger _log = BottleLogger.Instance;
        BottleDataRepository _repo;

        double _timecounter = 0.0;
        
        public BottleChartViewModel()
        {
            PlotModel = new PlotModel();

            _repo = BottleDataRepository.Instance;

            Points = new ObservableCollection<DataPoint>();
            _repo.Updated += UpdateMessages;
        }

        public PlotModel PlotModel { get; private set; }

        public ObservableCollection<DataPoint> Points { get; private set; }

        private void UpdateMessages(object sender, BottleDataEventArgs e)
        {
            Console.WriteLine("Adding Plot Item: {0}", _timecounter);

            App.Current.Dispatcher.InvokeAsync((Action)(() =>
            {
                _timecounter += 0.05;
                Points.Add(new DataPoint(_timecounter, e._bottlevalue));

                PlotModel.InvalidatePlot(true);
            }));

        }
    }
}
