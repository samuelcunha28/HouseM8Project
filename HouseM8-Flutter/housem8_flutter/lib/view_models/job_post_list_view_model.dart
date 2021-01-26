import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/services/job_post_web_service.dart';

import 'job_post_view_model.dart';

class JobPostListViewModel extends ChangeNotifier {
  List<JobPostViewModel> jobPosts = List<JobPostViewModel>();

  Future<void> fetchJobPosts() async {
    final returned = await JobPostWebService().fetchJobPosts();
    this.jobPosts =
        returned.map((post) => JobPostViewModel(jobPost: post)).toList();
    notifyListeners();
  }

  Future<void> ignoreJobPost([int index]) async {
    JobPostViewModel tmp = jobPosts[index];
    await JobPostWebService().ignoreJobPost(tmp.id);
  }

  Future<void> makeOffer([int index, double price]) async {
    JobPostViewModel tmp = jobPosts[index];
    await JobPostWebService().makeOffer(tmp.id, tmp.initialPrice);
    await JobPostWebService().ignoreJobPost(tmp.id);
  }
}
