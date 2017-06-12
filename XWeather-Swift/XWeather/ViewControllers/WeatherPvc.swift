//
//  WeatherPvc.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import UIKit

class WeatherPvc: UIPageViewController, UIPageViewControllerDataSource, UIPageViewControllerDelegate {

	@IBOutlet var emptyView: UIView!
	@IBOutlet var toolbarView: UIView!
	@IBOutlet weak var loadingContainerView: UIView!
	@IBOutlet weak var loadingIndicatorView: UIActivityIndicatorView!
	@IBOutlet weak var pageIndicator: UIPageControl!
	@IBOutlet var toolbarButtons: [UIButton]!
	
	var controllers = [UITableViewController]()
	
	@IBAction func closeClicked(_ sender: Any) {
	}
	
	@IBAction func settingsClicked(_ sender: Any) {
	}
	
	
	override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
	
	
	func updateBackground() {
		
		let location = ""
		
		let random = location == nil || Settings.randomBackgrounds
		
		var layer = view.layer.sublayers?[0] as? CAGradientLayer
		
		if layer == nil {
			layer = CAGradientLayer()
			layer!.frame = view.bounds
			view.layer.insertSublayer(layer!, at: 0)
		}
		
		var gradients = location.GetTimeOfDayGradient (random);
		
		if (layer?.Colors?.Length > 0 && layer.Colors [0] == gradients.Item1 [0] && layer.Colors [1] == gradients.Item1 [1])
		return;
		
		CATransaction.begin()
		CATransaction.setAnimationDuration(1.5)
		layer?.colors = gradients.Item1
		CATransaction.commit ()
	}
	
	
	func initControllers() {
		
		if let dailyTvc = storyboard?.instantiateViewController(withIdentifier: "DailyTvc") as? UITableViewController,
			let hourlyTvc = storyboard?.instantiateViewController(withIdentifier: "HourlyTvc") as? UITableViewController,
			let detailsTvc = storyboard?.instantiateViewController(withIdentifier: "DetailsTvc") as? UITableViewController {
			
			controllers += [ dailyTvc, hourlyTvc, detailsTvc ]
		}
	}
	
	
	func initToolbar() {
		
		toolbarView.translatesAutoresizingMaskIntoConstraints = false
		
		navigationController?.view.addSubview(toolbarView)
		
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "H:|[toolbarView]|", options: .directionLeadingToTrailing, metrics: [:], views: ["toolbarView": toolbarView]))
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "V:[toolbarView(44.0)]|", options: .directionLeadingToTrailing, metrics: [:], views: ["toolbarView": toolbarView]))
	}
	
	
	
	// MARK: - UIPageViewControllerDataSource
	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController? {
		return pageViewController.isEqual(controllers[2]) ? controllers[1] : pageViewController.isEqual(controllers[1]) ? controllers[0] : nil
	}
	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController? {
		return pageViewController.isEqual(controllers[0]) ? controllers[1] : pageViewController.isEqual(controllers[1]) ? controllers[2] : nil
	}
	
	
	// MARK: - UIPageViewControllerDelegate
	
	func pageViewController(_ pageViewController: UIPageViewController, willTransitionTo pendingViewControllers: [UIViewController]) {
		updateBackground()
	}
	
	func pageViewController(_ pageViewController: UIPageViewController, didFinishAnimating finished: Bool, previousViewControllers: [UIViewController], transitionCompleted completed: Bool) {
		if let indexController = pageViewController.viewControllers?[0] as? UITableViewController, let index = controllers.index(of: indexController) {
			pageIndicator.currentPage = index
			Settings.weatherPage = index
		}
	}
}
