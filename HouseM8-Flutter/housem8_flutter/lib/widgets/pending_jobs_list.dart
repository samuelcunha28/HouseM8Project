import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/pages/employer_review_page.dart';
import 'package:housem8_flutter/pages/mate_review_page.dart';
import 'package:housem8_flutter/pages/payment_page.dart';
import 'package:housem8_flutter/view_models/pending_job_list_view_model.dart';
import 'package:housem8_flutter/view_models/pending_job_view_model.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:housem8_flutter/widgets/remove_alert_dialog.dart';
import 'package:provider/provider.dart';

class PendingJobsList extends StatefulWidget {
  final List<PendingJobViewModel> pendingJobs;
  final String role;

  const PendingJobsList({this.pendingJobs, this.role});

  @override
  _PendingJobsListState createState() =>
      _PendingJobsListState(this.pendingJobs, this.role);
}

class _PendingJobsListState extends State<PendingJobsList> {
  final List<PendingJobViewModel> pendingJobs;
  final String role;

  _PendingJobsListState(this.pendingJobs, this.role);

  @override
  Widget build(BuildContext context) {
    if (this.role == "MATE") {
      return ListView.builder(
          itemCount: this.pendingJobs.length,
          itemBuilder: (context, index) {
            final jobs = this.pendingJobs[index];
            return Card(
              child: ListTile(
                title: Text(
                  jobs.pendingJobs.title.split('.').last,
                  style: TextStyle(fontSize: 16.0, color: Color(0xFF2F4858)),
                ),
                subtitle: Text(
                  jobs.pendingJobs.description,
                  style: TextStyle(fontSize: 14.0, color: Color(0xFF5B82AA)),
                ),
                trailing: IconButton(
                    icon: Icon(Icons.assignment_turned_in),
                    color: Color(0xFF5B82AA),
                    iconSize: 30,
                    tooltip: 'Trabalho concluído',
                    onPressed: () async {
                      var value = await showDialog(
                          context: context,
                          builder: (context) {
                            return RemoveAlertDialog("Concluir Trabalho",
                                "Tem a certeza que quer marcar este trabalho como concluído?");
                          });
                      if (value == "Sim") {
                        final vm = Provider.of<PendingJobListViewModel>(context,
                            listen: false);
                        int jobId = vm.pendingJobs[index].pendingJobs.jobId;
                        vm.markWorkAsComplete(index);
                        int employerId = await vm.fetchEmployerId(jobId);
                        await Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => EmployerReviewPage(
                                    employerId: employerId,
                                  )),
                        );
                        setState(() {});
                      }
                    }),
              ),
            );
          });
    } else {
      return ListView.builder(
          itemCount: this.pendingJobs.length,
          itemBuilder: (context, index) {
            final jobs = this.pendingJobs[index];

            return Card(
              child: ListTile(
                title: Text(
                  jobs.pendingJobs.title.split('.').last,
                  style: TextStyle(fontSize: 16.0, color: Color(0xFF00171F)),
                ),
                subtitle: Text(
                  jobs.pendingJobs.description,
                  style: TextStyle(fontSize: 14.0, color: Color(0xFF006064)),
                ),
                trailing: IconButton(
                  icon: Icon(Icons.assignment_turned_in),
                  color: Color(0xFF006064),
                  iconSize: 30,
                  tooltip: 'Trabalho concluído',
                  onPressed: () async {
                    var value = await showDialog(
                        context: context,
                        builder: (context) {
                          return RemoveAlertDialog("Concluir Trabalho",
                              "Tem a certeza que quer marcar este trabalho como concluído?");
                        });
                    if (value == "Sim") {
                      final vm = Provider.of<PendingJobListViewModel>(context,
                          listen: false);
                      int jobId = vm.pendingJobs[index].pendingJobs.jobId;
                      bool result = await vm.checkIfJobIsMarkedAsDone(jobId);
                      if (result) {
                        await vm.markWorkAsComplete(index);
                        double price = await vm.fetchCurrentJobPostPrice(jobId);
                        String url = await vm.fetchPaymentUrl(jobId, price);
                        await Navigator.of(context).push(MaterialPageRoute(
                          builder: (context) => PaymentPage(url: url),
                        ));
                        int mateId = await vm.fetchMateId(jobId);
                        await Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => MateReviewPage(
                                    mateId: mateId,
                                  )),
                        );
                      } else {
                        showDialog(
                            context: context,
                            builder: (context) {
                              return ErrorMessageDialog(
                                title: "Trabalho não concluído!",
                                text:
                                    "O mate ainda não marcou o trabalho como concluído!",
                              );
                            });
                      }
                      setState(() {});
                    }
                  },
                ),
              ),
            );
          });
    }
  }
}
