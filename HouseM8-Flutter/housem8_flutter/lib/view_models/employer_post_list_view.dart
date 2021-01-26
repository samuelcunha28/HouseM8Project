import 'package:flutter/material.dart';
import 'package:housem8_flutter/models/job_post_publication.dart';
import 'package:housem8_flutter/services/job_post_web_service.dart';

import 'employer_post_view.dart';

class EmployerPostListViewModel extends ChangeNotifier {
  List<EmployerPostViewModel> employerPosts = List<EmployerPostViewModel>();

  Future<void> fetchEmployerPosts() async {
    final returned = await JobPostWebService().fetchEmployerPosts();
    this.employerPosts = returned
        .map((post) => EmployerPostViewModel(employerPost: post))
        .toList();
    notifyListeners();
  }

  Future<void> addEmployerPost(JobPostPublication post) async {
    await JobPostWebService().createJobPost(post);
  }

  Future<void> deleteEmployerPost(int id) async {
    await JobPostWebService().deleteJobPost(id);
  }

  Future<void> uploadEmployerPost(JobPostPublication post, int id) async {
    await JobPostWebService().updateJobPost(post, id);
  }
}
