import 'package:flutter/material.dart';
import 'package:housem8_flutter/view_models/reviews_view_model.dart';

class MateReviewsList extends StatelessWidget {
  final List<ReviewsViewModel> mateReviews;

  MateReviewsList({this.mateReviews});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      itemCount: this.mateReviews.length,
      itemBuilder: (context, index) {
        final reviews = this.mateReviews[index];

        return Card(
          child: ListTile(
            title: Text(
              reviews.reviews.comment,
              style: TextStyle(
                  fontSize: 16.0,
                  color: Color(Colors.black.value),
                  fontWeight: FontWeight.w700),
            ),
            subtitle: Text(
              "Rating: " + reviews.reviews.rating.toString() + " estrelas!",
              style:
                  TextStyle(fontSize: 14.0, color: Color(Colors.black.value)),
            ),
          ),
        );
      },
    );
  }
}
