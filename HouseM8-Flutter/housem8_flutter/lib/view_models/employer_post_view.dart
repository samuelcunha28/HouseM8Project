import 'package:housem8_flutter/models/job_post.dart';

class EmployerPostViewModel {
  final JobPost employerPost;

  EmployerPostViewModel({this.employerPost});

  int get id {
    return this.employerPost.id;
  }

  String get title {
    return this.employerPost.title;
  }

  String get description {
    return this.employerPost.description;
  }

  String get address {
    return this.employerPost.address;
  }

  int get range {
    return this.employerPost.range;
  }

  double get initialPrice {
    return this.employerPost.initialPrice;
  }
}
