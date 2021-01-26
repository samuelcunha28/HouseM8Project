import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/models/pending_jobs.dart';
import 'package:housem8_flutter/services/payment_service.dart';
import 'package:housem8_flutter/services/pending_jobs_service.dart';
import 'package:housem8_flutter/services/reviews_service.dart';
import 'package:housem8_flutter/services/work_service.dart';
import 'package:housem8_flutter/view_models/pending_job_view_model.dart';

class PendingJobListViewModel extends ChangeNotifier {
  List<PendingJobViewModel> pendingJobs = List<PendingJobViewModel>();

  Future<void> fetchPendingJob() async {
    final returned = await PendingJobsService().fetchJobPosts();
    this.pendingJobs =
        returned.map((job) => PendingJobViewModel(pendingJobs: job)).toList();
    notifyListeners();
  }

  Future<bool> checkIfJobIsMarkedAsDone(int jobId) async {
    return await WorkService().isJobMarkedAsDone(jobId);
  }

  Future<double> fetchCurrentJobPostPrice(int jobId) async {
    return await PaymentService().fetchCurrentPrice(jobId);
  }

  Future<String> fetchPaymentUrl(int jobId, double price) async {
    return await PaymentService().makePayment(jobId, price);
  }

  Future<int> fetchEmployerId(int jobId) async {
    return ReviewsService().fetchEmployerId(jobId);
  }

  Future<int> fetchMateId(int jobId) async {
    return ReviewsService().fetchMateId(jobId);
  }

  Future<void> fetchEmployerPendingJob() async {
    final returned = await PendingJobsService().fetchEmployerPendingJobs();
    this.pendingJobs =
        returned.map((job) => PendingJobViewModel(pendingJobs: job)).toList();
    notifyListeners();
  }

  Future<void> markWorkAsComplete(int index) async {
    PendingJobs jobCompleted = pendingJobs[index].pendingJobs;
    pendingJobs.removeAt(index);
    notifyListeners();
    await WorkService().markJobAsDone(jobCompleted.jobId);
  }
}
