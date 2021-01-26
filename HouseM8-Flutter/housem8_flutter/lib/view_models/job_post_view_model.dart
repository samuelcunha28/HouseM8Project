import 'package:housem8_flutter/models/job_post.dart';

class JobPostViewModel {
  final JobPost jobPost;

  JobPostViewModel({this.jobPost});

  int get id {
    return this.jobPost.id;
  }

  String get title {
    return this.jobPost.title;
  }

  String get description {
    return this.jobPost.description;
  }

  String get address {
    return this.jobPost.address;
  }

  int get employerId {
    return this.jobPost.employerId;
  }

  int get range {
    return this.jobPost.range;
  }

  double get initialPrice {
    return this.jobPost.initialPrice;
  }
}
