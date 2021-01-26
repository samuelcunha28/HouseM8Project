import 'package:housem8_flutter/models/mate_review.dart';

class ReviewsViewModel {
  final MateReviews reviews;

  ReviewsViewModel({this.reviews});

  String get comment {
    return this.reviews.comment;
  }

  double get rating {
    return this.reviews.rating;
  }
}
